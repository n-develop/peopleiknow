using System.Threading.Tasks;
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
        
        public Task SendReminders()
        {
            throw new System.NotImplementedException();
        }
    }
}