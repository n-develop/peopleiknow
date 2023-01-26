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

        public async Task SendReminders(DateTime reminderDate)
        {
            await SendGeneralRemindersAsync(reminderDate);
            await SendBirthdayRemindersAsync(reminderDate);
        }

        private async Task SendGeneralRemindersAsync(DateTime reminderDate)
        {
            var reminders = _reminderRepository.GetAll().Include(r => r.Contact)
                .Where(r => r.Date == reminderDate
                            || (r.RemindMeEveryYear && r.Date.Day == reminderDate.Day &&
                                r.Date.Month == reminderDate.Month)).ToList();
            foreach (var reminder in reminders)
            {
                await _messagingService.SendMessageAsync("⏰ " + reminder.Description,
                    $"{reminder.Description} reminder for {reminder.Contact.FullName}");
            }
        }

        private async Task SendBirthdayRemindersAsync(DateTime reminderDate)
        {
            var birthdayContacts = await _contactRepository.GetBirthdayContactsAsync(reminderDate);
            foreach (var birthdayContact in birthdayContacts)
            {
                if (birthdayContact.SendBirthdayNotification)
                {
                    await _messagingService.SendMessageAsync("🎁 " + birthdayContact.FullName,
                        $"It's {birthdayContact.FullName}'s birthday");    
                }
            }
        }
    }
}