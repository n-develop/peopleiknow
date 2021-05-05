using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PeopleIKnow
{
    public class NotificationHostedService : IHostedService, IDisposable
    {
        private Timer _timer;
        private ILogger<NotificationHostedService> _logger;

        public NotificationHostedService(ILogger<NotificationHostedService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var daily = TimeSpan.FromHours(24);
            var todaysScheduled = DateTime.Today.AddHours(6).AddMinutes(34);
            var nextRunTime = todaysScheduled > DateTime.Now ? todaysScheduled : todaysScheduled.AddDays(1);
            var timeUntilFirstRuntime = nextRunTime.Subtract(DateTime.Now);

            void FireTaskAtSchedule()
            {
                var firstExecutionDelay = Task.Delay(timeUntilFirstRuntime, cancellationToken);
                firstExecutionDelay.Wait(cancellationToken);
                DoWork(null);
                _timer = new Timer(DoWork, null, TimeSpan.Zero, daily);
            }

            Task.Run((Action) FireTaskAtSchedule, cancellationToken);
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("NotificationHostedService is working");
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