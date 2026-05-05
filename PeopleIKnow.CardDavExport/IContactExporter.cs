using System.Threading.Tasks;
using FolkerKinzel.VCards;

namespace PeopleIKnow.CardDavExport
{
    public interface IContactExporter
    {
        Task ExportAsync(VCard vcard, string uid);
    }
}
