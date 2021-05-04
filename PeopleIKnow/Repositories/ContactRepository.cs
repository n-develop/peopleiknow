using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeopleIKnow.Models;

namespace PeopleIKnow.Repositories
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
                .Include(c => c.Activities)
                .FirstOrDefault(c => c.Id == id);
            return contact ?? NullContact.GetInstance();
        }

        public bool DeleteContact(int id)
        {
            var contact = GetContactById(id);
            if (contact.IsNull())
            {
                _logger.LogInformation($"Contact with ID {id} could not be found for deletion");
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

            _context.Contacts.Remove(contact);

            _logger.LogInformation($"Contact with ID {id} successfully deleted");

            _context.SaveChanges();
            return true;
        }

        public Contact AddContact(Contact contact)
        {
            var maxId = _context.Contacts.Max(c => c.Id);
            contact.Id = maxId + 1;

            var entry = _context.Contacts.Add(contact);
            _context.SaveChanges();
            _logger.LogInformation($"New contact with ID {contact.Id} was created");
            return entry.Entity;
        }

        public void SaveContact(Contact contact)
        {
            _context.Contacts.Update(contact);
            _context.SaveChanges();
            _logger.LogInformation($"Contact with ID {contact.Id} was saved");
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