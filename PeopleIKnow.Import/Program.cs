using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PeopleIKnow.Import
{
    class Program
    {
        static void Main(string[] args)
        {
            var collection = new ServiceCollection();
            collection.AddDbContext<ContactContext>(
                options => options.UseSqlite("Data Source=../PeopleIKnow/people.db"));
            var serviceProvider = collection.BuildServiceProvider();

            var db = serviceProvider.GetService<ContactContext>();

            var filePath = "/Users/larsrichter/Downloads/contacts.csv";
            var importer = new CsvImporter(db);
            Console.WriteLine("Importing contacts...");
            var contactsImported = importer.ImportFromFile(filePath);
            Console.WriteLine($"{contactsImported} contacts were imported");
            Console.ReadKey();
        }
    }
}