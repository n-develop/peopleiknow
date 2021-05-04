using System.Collections.Generic;
using System.Threading.Tasks;
using PeopleIKnow.Models;

namespace PeopleIKnow.Repositories
{
    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> SearchContacts(string term);
        Task<IEnumerable<Contact>> GetAllContacts();
        Task<IEnumerable<Contact>> GetContacts(int skip, int take);
        Contact GetContactById(int id);
        bool DeleteContact(int id);
        Contact AddContact(Contact contact);
        void SaveContact(Contact contact);
    }
}