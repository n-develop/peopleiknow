using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;
using PeopleIKnow.Services;

namespace PeopleIKnow.UnitTests.TelegramNotificationServiceTests
{
    public class SendMessage
    {
        #region test infrastructure

        private ILogger<TelegramNotificationService> _logger;
        private IOptions<NotificationSettings> _options;
        private MockHttpMessageHandler _messageHandler;

        public SendMessage()
        {
            _logger = Substitute.For<ILogger<TelegramNotificationService>>();
            _options = Substitute.For<IOptions<NotificationSettings>>();
            _messageHandler = new MockHttpMessageHandler();
        }

        private TelegramNotificationService CreateService()
        {
            return new TelegramNotificationService(new HttpClient(_messageHandler), _options, _logger);
        }

        #endregion

        [Fact]
        public async Task DisabledNotifications_DoesNotCallTelegram()
        {
            // Arrange
            _options.Value.Returns(new NotificationSettings
            {
                Enabled = false
            });
            var service = CreateService();

            // Act
            await service.SendMessageAsync("not important", "not important");

            // Assert
            _messageHandler.Calls.Should().Be(0);
        }
    }
}