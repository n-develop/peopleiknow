using System;
using System.Threading.Tasks;

namespace PeopleIKnow.Services
{
    public interface IReminderService
    {
        Task SendReminders(DateTime reminderDate);
    }
}