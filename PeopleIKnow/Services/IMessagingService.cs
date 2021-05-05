using System.Threading.Tasks;

namespace PeopleIKnow.Services
{
    public interface IMessagingService
    {
        Task SendMessageAsync(string title, string message);
    }
}