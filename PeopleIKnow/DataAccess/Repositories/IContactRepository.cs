using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PeopleIKnow.Models;

namespace PeopleIKnow.DataAccess.Repositories
{
    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> SearchContacts(string term);
        Task<IEnumerable<Contact>> GetAllContacts();
        Task<IEnumerable<Contact>> GetContacts(int skip, int take);
        Contact GetContactById(int id);
        bool DeleteContact(int id);
        Task<Contact> AddContact(Contact contact);
        Task SaveContact(Contact contact);
        Task<IEnumerable<Contact>> GetBirthdayContactsAsync(DateTime birthday);
    }
}