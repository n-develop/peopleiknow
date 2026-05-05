using System;
using System.IO;
using System.Threading.Tasks;
using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;

namespace PeopleIKnow.CardDavExport
{
    public class FileExporter : IContactExporter
    {
        private readonly string _outputDirectory;

        public FileExporter(string outputDirectory)
        {
            _outputDirectory = outputDirectory;
            Directory.CreateDirectory(_outputDirectory);
        }

        public Task ExportAsync(VCard vcard, string uid)
        {
            var filePath = Path.Combine(_outputDirectory, $"{uid}.vcf");
            Vcf.Save(new[] { vcard }, filePath, VCdVersion.V3_0);
            Console.WriteLine($"  Written: {filePath}");
            return Task.CompletedTask;
        }
    }
}
