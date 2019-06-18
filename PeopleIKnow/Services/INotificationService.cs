using System.Threading.Tasks;

namespace PeopleIKnow.Services
{
    public interface INotificationService
    {
        Task SendMessageAsync(string title, string message);
    }
}