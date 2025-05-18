using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Ordering.API.Extensions;

public static class DbExtension
{
    public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder)
        where TContext : DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();            try
            {
                logger.LogInformation($"Started Db Migration: {typeof(TContext).Name}");
                logger.LogInformation($"Connection string: {context.Database.GetConnectionString()}");
                
                //retry strategy
                var retry = Policy.Handle<SqlException>()
                    .WaitAndRetry(
                        retryCount: 5,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (exception, span, count) =>
                        {
                            logger.LogError($"Retry attempt {count} because of: {exception.Message}");
                            if (exception is SqlException sqlEx)
                            {
                                logger.LogError($"SQL Error Number: {sqlEx.Number}, State: {sqlEx.State}");
                            }
                        });
                retry.Execute(() => CallSeeder(seeder, context, services));
                logger.LogInformation($"Migration Completed: {typeof(TContext).Name}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An Error occurred while migrating db: {typeof(TContext).Name}");
                // Additional error details for troubleshooting
                if (ex.InnerException != null)
                {
                    logger.LogError(ex.InnerException, $"Inner exception: {ex.InnerException.Message}");
                }
            }
        }
        return host;
    }    
    
    private static void CallSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext? context, IServiceProvider services) where TContext : DbContext
    {
        // Ensure the database exists
        context.Database.EnsureCreated();
        
        // Apply migrations
        context.Database.Migrate();
        
        // Execute the seeder
        seeder(context, services);
    }
}
