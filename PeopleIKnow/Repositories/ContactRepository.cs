using System;
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
            var contact = GetContactById(id);
            if (contact.IsNull())
            {
                return false;
            }

            foreach (var emailAddress in contact.EmailAddresses)
            {
                _context.EmailAddresses.Remove(emailAddress);
            }

            foreach (var statusUpdate in contact.StatusUpdates)
            {
                _context.StatusUpdates.Remove(statusUpdate);
            }

            foreach (var relationship in contact.Relationships)
            {
                _context.Relationships.Remove(relationship);
            }

            foreach (var telephoneNumber in contact.TelephoneNumbers)
            {
                _context.TelephoneNumbers.Remove(telephoneNumber);
            }

            _context.Contacts.Remove(contact);

            _context.SaveChanges();
            return true;
        }

        public Contact AddContact(Contact contact)
        {
            var maxId = _context.Contacts.Max(c => c.Id);
            contact.Id = maxId + 1;

            var entry = _context.Contacts.Add(contact);
            _context.SaveChanges();

            return entry.Entity;
        }

        public void SaveContact(Contact contact)
        {
            _context.Contacts.Update(contact);
            _context.SaveChanges();
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

        public void UpdateEmail(EmailAddress mail)
        {
            if (mail.IsNull() || mail.Id <= 0)
            {
                return;
            }

            var emailFromRepository = _context.EmailAddresses.Find(mail.Id);
            if (emailFromRepository == null)
            {
                return;
            }

            emailFromRepository.Type = mail.Type;
            emailFromRepository.Email = mail.Email;

            _context.SaveChanges();
        }

        public void UpdateRelationship(Relationship relationship)
        {
            if (relationship.IsNull() || relationship.Id <= 0)
            {
                return;
            }

            var relationshipFromRepository = _context.Relationships.Find(relationship.Id);
            if (relationshipFromRepository == null)
            {
                return;
            }

            relationshipFromRepository.Type = relationship.Type;
            relationshipFromRepository.Person = relationship.Person;

            _context.SaveChanges();
        }

        public void UpdateTelephoneNumber(TelephoneNumber telephoneNumber)
        {
            if (telephoneNumber.IsNull() || telephoneNumber.Id <= 0)
            {
                return;
            }

            var telephoneNumberFromRepository = _context.TelephoneNumbers.Find(telephoneNumber.Id);
            if (telephoneNumberFromRepository == null)
            {
                return;
            }

            telephoneNumberFromRepository.Type = telephoneNumber.Type;
            telephoneNumberFromRepository.Telephone = telephoneNumber.Telephone;

            _context.SaveChanges();
        }

        public StatusUpdate GetStatusUpdateById(int id)
        {
            if (id <= 0)
            {
                return NullStatusUpdate.GetInstance();
            }

            var statusUpdate = _context.StatusUpdates.Find(id);
            if (statusUpdate == null)
            {
                return NullStatusUpdate.GetInstance();
            }

            return statusUpdate;
        }

        public void UpdateStatusUpdate(StatusUpdate statusUpdate)
        {
            if (statusUpdate.IsNull() || statusUpdate.Id <= 0)
            {
                return;
            }

            var statusFromRepository = _context.StatusUpdates.Find(statusUpdate.Id);
            if (statusFromRepository == null)
            {
                return;
            }

            statusFromRepository.Created = statusUpdate.Created;
            statusFromRepository.StatusText = statusUpdate.StatusText;

            _context.SaveChanges();
        }

        public void DeleteStatusUpdate(StatusUpdate statusUpdate)
        {
            _context.StatusUpdates.Remove(statusUpdate);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Contact>> SearchContacts(string term)
        {
            var allContacts = await GetAllContacts();
            var filtered = allContacts.Where(c =>
                    c.Firstname != null && c.Firstname.Contains(term, StringComparison.InvariantCultureIgnoreCase)
                    || c.Lastname != null && c.Lastname.Contains(term, StringComparison.InvariantCultureIgnoreCase)
                    || c.Middlename != null && c.Middlename.Contains(term, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
            return filtered;
        }
    }
}