using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PeopleIKnow.Models;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.Services
{
    public class ReminderService : IReminderService
    {
        private readonly IMessagingService _messagingService;
        private readonly IRepository<Reminder> _repository;

        public ReminderService(IMessagingService messagingService, IRepository<Reminder> repository)
        {
            _messagingService = messagingService;
            _repository = repository;
        }

        public async Task SendReminders()
        {
            var reminders = _repository.GetAll().Include(r => r.Contact)
                .Where(r => r.Date == DateTime.Today).ToList();
            foreach (var reminder in reminders)
            {
                await _messagingService.SendMessageAsync("⏰ " + reminder.Description,
                    $"{reminder.Description} reminder for {reminder.Contact.FullName}");
            }
        }
    }
}