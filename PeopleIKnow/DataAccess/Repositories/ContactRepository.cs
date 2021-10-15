using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeopleIKnow.Models;

namespace PeopleIKnow.DataAccess.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactContext _context;
        private readonly ILogger<ContactRepository> _logger;

        public ContactRepository(ContactContext context, ILogger<ContactRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Contact>> GetAllContacts()
        {
            return await _context.Contacts
                .Include(contact => contact.EmailAddresses)
                .AsNoTracking().ToListAsync();
        }

        public Contact GetContactById(int id)
        {
            var contact = _context.Contacts
                .Include(c => c.EmailAddresses)
                .Include(c => c.TelephoneNumbers)
                .Include(c => c.Relationships)
                .Include(c => c.StatusUpdates)
                .Include(c => c.Activities)
                .Include(c => c.Gifts)
                .Include(c => c.Reminders)
                .FirstOrDefault(c => c.Id == id);
            return contact ?? NullContact.GetInstance();
        }

        public bool DeleteContact(int id)
        {
            var contact = GetContactById(id);
            if (contact.IsNull())
            {
                _logger.LogInformation("Contact with ID {Id} could not be found for deletion", id);
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

            foreach (var commonActivity in contact.Activities)
            {
                _context.CommonActivities.Remove(commonActivity);
            }

            foreach (var relationship in contact.Relationships)
            {
                _context.Relationships.Remove(relationship);
            }

            foreach (var telephoneNumber in contact.TelephoneNumbers)
            {
                _context.TelephoneNumbers.Remove(telephoneNumber);
            }

            foreach (var gift in contact.Gifts)
            {
                _context.Gifts.Remove(gift);
            }

            foreach (var reminder in contact.Reminders)
            {
                _context.Reminders.Remove(reminder);
            }

            _context.Contacts.Remove(contact);

            _logger.LogInformation("Contact with ID {Id} successfully deleted", id);

            _context.SaveChanges();
            return true;
        }

        public async Task<Contact> AddContact(Contact contact)
        {
            var entry = await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();
            _logger.LogInformation("New contact with ID {Id} was created", entry.Entity.Id);
            return entry.Entity;
        }

        public async Task SaveContact(Contact contact)
        {
            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Contact with ID {ContactId} was saved", contact.Id);
        }

        public async Task<IEnumerable<Contact>> GetBirthdayContactsAsync(DateTime birthday)
        {
            return await _context.Contacts.Where(c =>
                    c.Birthday != DateTime.MinValue &&
                    c.Birthday.Day == birthday.Day &&
                    c.Birthday.Month == birthday.Month)
                .ToListAsync();
        }

        public async Task<IEnumerable<Contact>> SearchContacts(string term)
        {
            var allContacts = await GetAllContacts();

            var filtered = allContacts
                .Where(c => c.FullName.Contains(term, StringComparison.InvariantCultureIgnoreCase)
                            || !string.IsNullOrEmpty(c.Tags) && c.Tags.Contains(term,
                                StringComparison.InvariantCultureIgnoreCase))
                .ToList();
            return filtered;
        }
    }
}