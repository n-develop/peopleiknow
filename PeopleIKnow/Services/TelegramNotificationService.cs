using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace PeopleIKnow.Services
{
    public class TelegramNotificationService : INotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly NotificationSettings _notificationSettings;

        private const string ApiUrl =
            "https://api.telegram.org/bot{0}/sendMessage?chat_id=10190803&parse_mode=Markdown&text={1}";

        private const string MessageTemplate = "*{0}*\n\n{1}";

        public TelegramNotificationService(HttpClient httpClient,
            IOptions<NotificationSettings> notificationSettings,
            ILogger<TelegramNotificationService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _notificationSettings = notificationSettings.Value;
        }

        public async Task SendMessageAsync(string title, string message)
        {
            if (!_notificationSettings.Enabled)
            {
                return;
            }

            try
            {
                var text = string.Format(MessageTemplate, title, message);
                await _httpClient.GetAsync(string.Format(ApiUrl, _notificationSettings,
                    System.Net.WebUtility.UrlEncode(text)));
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, $"Notification could not be send.\nTitle: {title}\nMessage: {message}");
            }
        }
    }
}