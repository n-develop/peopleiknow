using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PeopleIKnow.DataAccess.Repositories;
using PeopleIKnow.Models;

namespace PeopleIKnow.Services
{
    public class ReminderService : IReminderService
    {
        private readonly IMessagingService _messagingService;
        private readonly IRepository<Reminder> _reminderRepository;
        private readonly IContactRepository _contactRepository;

        public ReminderService(IMessagingService messagingService, IRepository<Reminder> reminderRepository,
            IContactRepository contactRepository)
        {
            _messagingService = messagingService;
            _reminderRepository = reminderRepository;
            _contactRepository = contactRepository;
        }

        public async Task SendReminders()
        {
            await SendGeneralRemindersAsync();
            await SendBirthdayRemindersAsync();
        }

        private async Task SendGeneralRemindersAsync()
        {
            var reminders = _reminderRepository.GetAll().Include(r => r.Contact)
                .Where(r => r.Date == DateTime.Today).ToList();
            foreach (var reminder in reminders)
            {
                await _messagingService.SendMessageAsync("⏰ " + reminder.Description,
                    $"{reminder.Description} reminder for {reminder.Contact.FullName}");
            }
        }

        private async Task SendBirthdayRemindersAsync()
        {
            var birthdayContacts = await _contactRepository.GetBirthdayContacts(DateTime.Today);
            foreach (var birthdayContact in birthdayContacts)
            {
                await _messagingService.SendMessageAsync("🎁 " + birthdayContact.FullName,
                    $"It's {birthdayContact.FullName}'s birthday");
            }
        }
    }
}