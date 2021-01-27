using CreditOne.Microservices.BuildingBlocks.NotificationAgents;
using CreditOne.Microservices.BuildingBlocks.PerformanceMonitor.Configuration;
using CreditOne.Microservices.BuildingBlocks.PerformanceMonitor.Database;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CreditOne.Microservices.BuildingBlocks.ServiceCollectionExtensions
{
    /// <summary>
    /// Implements custom service extension methods
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///		<term>Date</term>
    ///		<term>Who</term>
    ///		<term>BR/WO</term>
    ///		<description>Description</description>
    /// </listheader>
    /// <item>
    ///		<term>10/28/2019</term>
    ///		<term>Daniel Lobaton</term>
    ///		<term>RM-47</term>
    ///		<description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks>
    public static class DbPerformanceMonitorServiceCollectionExtensions
    {
        /// <summary>
        ///  Creates the <c>DatabaseAlertHandler</c> service that monitors database operation using Performance Counters
        ///  Then it is added to the service container.
        /// </summary>
        /// <param name="services">The services</param>
        /// <param name="configuration">The configurations</param>
        /// <returns>The service container</returns>
        public static IServiceCollection AddDbPerformanceMonitor(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<DatabaseAlertHandlerParametersConfiguration>(configuration.GetSection("DatabaseAlertHandlerParameters"));
            services.Configure<DatabaseTimeoutThresholdConfiguration>(configuration.GetSection("DatabaseTimeoutThresholdConfiguration"));
            services.Configure<PerformanceCounterCategoryConfiguration>(configuration.GetSection("PerformanceCounterCategory"));

            services.AddSingleton<IDatabaseMeasurement, DatabaseMeasurement>();
            services.AddSingleton<SmtpMailSender>();

            services.AddSingleton<DatabaseAlertHandler>();

            //Because it won't instanciate and won't create monitor
            services.BuildServiceProvider().GetRequiredService<DatabaseAlertHandler>();

            return services;
        }
    }
}
