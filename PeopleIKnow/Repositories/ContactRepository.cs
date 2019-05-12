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

        public EmailAddress GetEmailById(int mailId)
        {
            if (mailId <= 0)
            {
                return NullEmailAddress.GetInstance();
            }

            var mail = _context.EmailAddresses.Find(mailId);

            if (mail == null)
            {
                return NullEmailAddress.GetInstance();
            }

            return mail;
        }

        public void DeleteEmailAddress(EmailAddress mail)
        {
            _context.EmailAddresses.Remove(mail);
            _context.SaveChanges();
        }

        public void AddStatusUpdate(StatusUpdate statusUpdate)
        {
            if (statusUpdate.Id == 0)
            {
                var maxId = _context.StatusUpdates.Max(c => c.Id);
                statusUpdate.Id = maxId + 1;
            }

            _context.StatusUpdates.Add(statusUpdate);
            _context.SaveChanges();
        }

        public void AddTelephoneNumber(TelephoneNumber telephoneNumber)
        {
            if (telephoneNumber.Id == 0)
            {
                var maxId = _context.TelephoneNumbers.Max(c => c.Id);
                telephoneNumber.Id = maxId + 1;
            }

            _context.TelephoneNumbers.Add(telephoneNumber);
            _context.SaveChanges();
        }

        public TelephoneNumber GetTelephoneNumberById(int id)
        {
            if (id <= 0)
            {
                return NullTelephoneNumber.GetInstance();
            }

            var telephoneNumber = _context.TelephoneNumbers.Find(id);

            if (telephoneNumber == null)
            {
                return NullTelephoneNumber.GetInstance();
            }

            return telephoneNumber;
        }

        public void DeleteTelephoneNumber(TelephoneNumber telephoneNumber)
        {
            _context.TelephoneNumbers.Remove(telephoneNumber);
            _context.SaveChanges();
        }

        public void AddRelationship(Relationship relationship)
        {
            if (relationship.Id == 0)
            {
                var maxId = _context.Relationships.Max(c => c.Id);
                relationship.Id = maxId + 1;
            }

            _context.Relationships.Add(relationship);
            _context.SaveChanges();
        }

        public Relationship GetRelationshipById(int id)
        {
            if (id <= 0)
            {
                return NullRelationship.GetInstance();
            }

            var relationship = _context.Relationships.Find(id);

            if (relationship == null)
            {
                return NullRelationship.GetInstance();
            }

            return relationship;
        }

        public void DeleteRelationship(Relationship relationship)
        {
            _context.Relationships.Remove(relationship);
            _context.SaveChanges();
        }
    }
}