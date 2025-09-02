using Microsoft.EntityFrameworkCore;

namespace foodies_api.Utils;

public class DatabaseReadinessChecker<TContext> where TContext : DbContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly int _maxRetries;
    private readonly TimeSpan _baseDelay;

    public DatabaseReadinessChecker(IServiceProvider serviceProvider, int maxRetries = 5, TimeSpan? baseDelay = null)
    {
        _serviceProvider = serviceProvider;
        _maxRetries = maxRetries;
        _baseDelay = baseDelay ?? TimeSpan.FromSeconds(2);
    }

    public async Task WaitUntilReadyAsync(CancellationToken cancellationToken = default)
    {
        for (int attempt = 1; attempt <= _maxRetries; attempt++)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<TContext>();

                Console.WriteLine($"Database attempt {attempt}/{_maxRetries}: Testing connection...");

                // Test connection with a shorter timeout
                using var connectCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);
                await context.Database.CanConnectAsync(connectCts.Token);
                Console.WriteLine("Database connection successful!");

                // Ensure database is created with a longer timeout
                using var ensureCreatedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, new CancellationTokenSource(TimeSpan.FromSeconds(15)).Token);
                await context.Database.EnsureCreatedAsync(ensureCreatedCts.Token);
                Console.WriteLine("Database schema verified!");

                return; // Success, exit the loop
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database attempt {attempt} failed: {ex.Message}");

                if (attempt < _maxRetries)
                {
                    var delay = TimeSpan.FromSeconds(_baseDelay.TotalSeconds * attempt);
                    Console.WriteLine($"Retrying in {delay.TotalSeconds} seconds...");
                    await Task.Delay(delay, cancellationToken);
                }
                else
                {
                    Console.WriteLine("Database initialization failed after all retries. App will start but may have issues.");
                }
            }
        }
    }
}