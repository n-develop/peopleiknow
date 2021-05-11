using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PeopleIKnow.Configuration;
using PeopleIKnow.Services;

namespace PeopleIKnow
{
    public class NotificationHostedService : IHostedService, IDisposable
    {
        private Timer _timer;
        private ILogger<NotificationHostedService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly NotificationSettings _settings;

        public NotificationHostedService(ILogger<NotificationHostedService> logger, IServiceScopeFactory scopeFactory,
            IOptions<NotificationSettings> notificationSettings)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _settings = notificationSettings.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var daily = TimeSpan.FromHours(24);
            var todaysSchedule = DateTime.Today.AddHours(_settings.Time.Hour).AddMinutes(_settings.Time.Minute);
            var nextRunTime = todaysSchedule > DateTime.Now ? todaysSchedule : todaysSchedule.AddDays(1);
            var timeUntilFirstRuntime = nextRunTime.Subtract(DateTime.Now);
            _timer = new Timer(Notify, null, timeUntilFirstRuntime, daily);

            return Task.CompletedTask;
        }

        private async void Notify(object state)
        {
            using var scope = _scopeFactory.CreateScope();
            var reminderService = scope.ServiceProvider.GetRequiredService<IReminderService>();
            await reminderService.SendReminders(DateTime.Today);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("NotificationHostedService is stopping");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}