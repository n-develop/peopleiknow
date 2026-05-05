using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PeopleIKnow.DataAccess;

namespace PeopleIKnow.CardDavExport
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddCommandLine(args)
                .Build();

            var settings = new ExportSettings();
            configuration.GetSection("ExportSettings").Bind(settings);

            var collection = new ServiceCollection();
            collection.AddDbContext<ContactContext>(
                options => options.UseSqlite(settings.ConnectionString));
            var serviceProvider = collection.BuildServiceProvider();

            var db = serviceProvider.GetService<ContactContext>();

            var contacts = await db.Contacts
                .Include(c => c.TelephoneNumbers)
                .Include(c => c.EmailAddresses)
                .Include(c => c.Relationships)
                .Include(c => c.StatusUpdates)
                .ToListAsync();

            Console.WriteLine($"Found {contacts.Count} contacts to export.");

            var mapper = new ContactToVCardMapper(settings.WebRootPath);

            IContactExporter exporter;
            if (settings.ExportMode.Equals("carddav", StringComparison.OrdinalIgnoreCase))
            {
                exporter = new CardDavExporter(
                    settings.ServerUrl,
                    settings.AddressBookPath,
                    settings.Username,
                    settings.Password);
            }
            else
            {
                exporter = new FileExporter(settings.OutputDirectory);
            }

            int success = 0, failed = 0;

            foreach (var contact in contacts)
            {
                var fullName = contact.FullName;
                if (string.IsNullOrWhiteSpace(fullName))
                {
                    Console.WriteLine($"  Skipping contact {contact.Id} (no name)");
                    continue;
                }

                try
                {
                    var vcard = mapper.Map(contact);
                    var uid = $"pik-contact-{contact.Id}";
                    Console.Write($"Exporting: {fullName}...");
                    await exporter.ExportAsync(vcard, uid);
                    success++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  Error exporting {fullName}: {ex.Message}");
                    failed++;
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Export complete. Success: {success}, Failed: {failed}");

            if (exporter is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
