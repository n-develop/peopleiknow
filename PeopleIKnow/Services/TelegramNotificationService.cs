using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace PeopleIKnow.Services
{
    public class TelegramNotificationService : INotificationService
    {
        private readonly NotificationSettings _notificationSettings;

        private const string ApiUrl =
            "https://api.telegram.org/bot{0}/sendMessage?chat_id=10190803&parse_mode=Markdown&text={1}";

        private const string MessageTemplate = "*{0}*\n\n{1}";

        public TelegramNotificationService(IOptions<NotificationSettings> notificationSettings)
        {
            _notificationSettings = notificationSettings.Value;
        }

        public async Task SendMessageAsync(string title, string message)
        {
            if (!_notificationSettings.Enabled)
            {
                return;
            }

            using (var client = new HttpClient())
            {
                var text = string.Format(MessageTemplate, title, message);
                await client.GetAsync(string.Format(ApiUrl, _notificationSettings,
                    System.Net.WebUtility.UrlEncode(text)));
            }
        }
    }
}