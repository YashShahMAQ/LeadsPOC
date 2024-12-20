using PullLeadsMicroService.Services;
using System.Timers;

namespace PullLeadsMicroService.BackgroundTasks
{
    public class PullBackgroundService(ILogger<PullBackgroundService> logger, IServiceProvider serviceProvider) : IHostedService, IDisposable
    {
        private readonly ILogger<PullBackgroundService> _logger = logger;
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private System.Timers.Timer? _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("PullBackgroundService started.");

            // Initialize the timer
            _timer = new System.Timers.Timer(TimeSpan.FromMinutes(1).TotalMilliseconds); // 10-minute interval
            _timer.Elapsed += OnTimerElapsed;
            _timer.AutoReset = true; // Repeats the timer
            _timer.Enabled = true;

            return Task.CompletedTask;
        }
        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            _logger.LogInformation("Timer triggered at {Time}. Starting data pull...", DateTime.UtcNow);

            // Perform pull operation in a new DI scope
            using var scope = _serviceProvider.CreateScope();
            var pullService = scope.ServiceProvider.GetRequiredService<IPullService>();

            try
            {
                pullService.PullDataAsync().Wait(); // Execute the pull operation
                _logger.LogInformation("Data pull completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred during data pull: {Message}", ex.Message);
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("PullBackgroundService is stopping...");
            _timer?.Stop();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
