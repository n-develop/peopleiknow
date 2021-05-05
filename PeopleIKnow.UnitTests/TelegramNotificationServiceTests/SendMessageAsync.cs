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

        private ILogger<TelegramMessagingService> _logger;
        private IOptions<NotificationSettings> _options;
        private MockHttpMessageHandler _messageHandler;

        public SendMessage()
        {
            _logger = Substitute.For<ILogger<TelegramMessagingService>>();
            _options = Substitute.For<IOptions<NotificationSettings>>();
            _messageHandler = new MockHttpMessageHandler();
        }

        private TelegramMessagingService CreateService()
        {
            return new TelegramMessagingService(new HttpClient(_messageHandler), _options, _logger);
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

        [Fact]
        public async Task EnabledNotifications_CallsTelegram()
        {
            // Arrange
            _options.Value.Returns(new NotificationSettings
            {
                Enabled = true,
                Token = "not important"
            });
            var service = CreateService();

            // Act
            await service.SendMessageAsync("not important", "not important");

            // Assert
            _messageHandler.Calls.Should().Be(1);
        }

        [Fact]
        public async Task ExceptionIsThrown_DoesNotFail()
        {
            // Arrange
            _options.Value.Returns(new NotificationSettings
            {
                Enabled = true,
                Token = "not important"
            });
            _messageHandler = new FailingHttpMessageHandler();
            var service = CreateService();

            // Act
            await service.SendMessageAsync("not important", "not important");

            // Assert
            _messageHandler.Calls.Should().Be(1);
        }
    }
}