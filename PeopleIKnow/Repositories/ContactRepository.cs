using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PeopleIKnow.Models;

namespace PeopleIKnow.Repositories
{
    class ContactRepository : IContactRepository
    {
        private readonly ContactContext _context;

        public ContactRepository(ContactContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contact>> GetAllContacts()
        {
            return await _context.Contacts
                .Include(contact => contact.EmailAddresses)
                .AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Contact>> GetContacts(int skip, int take)
        {
            return await _context.Contacts.Include(contact => contact.EmailAddresses).AsNoTracking().Skip(skip)
                .Take(take).ToListAsync();
        }

        public Contact GetContactById(int id)
        {
            var contact = _context.Contacts
                .Include(c => c.EmailAddresses)
                .Include(c => c.TelephoneNumbers)
                .Include(c => c.Relationships)
                .Include(c => c.StatusUpdates)
                .FirstOrDefault(c => c.Id == id);
            return contact ?? NullContact.GetInstance();
        }

        public bool DeleteContact(int id)
        {
            throw new System.NotImplementedException();
        }

        public Contact AddContact(Contact contact)
        {
            throw new System.NotImplementedException();
        }

        public bool SaveContact(Contact contact)
        {
            _context.Contacts.Update(contact);
            _context.SaveChanges();
            return true;
        }

        public void AddEmail(EmailAddress mail)
        {
            if (mail.Id == 0)
            {
                var maxId = _context.EmailAddresses.Max(c => c.Id);
                mail.Id = maxId + 1;
            }

            _context.EmailAddresses.Add(mail);
            _context.SaveChanges();
        }
    }
}