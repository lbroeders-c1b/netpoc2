using CreditOne.Microservices.BuildingBlocks.Common.Configuration;

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
    public static class ApiEndPointsServiceCollectionExtensions
    {
        /// <summary>
        ///  Creates the <c>ApiEndPointsConfiguration</c> service which contains a <c>Dictionary</c> of
        ///  web api and the uri configuration.
        ///  Then it is added to the service container.
        /// </summary>
        /// <param name="services">The services</param>
        /// <param name="configuration">The configurations</param>
        /// <returns>The service container</returns>
        public static IServiceCollection AddApiEndPointsConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var uriConfiguration = new ConfigurationBuilder()
                .AddJsonFile(configuration.GetValue<string>("MsApiEndPointsPath")).Build();

            var apiEndPointsConfiguration = new ApiEndPointsConfiguration();
            uriConfiguration.Bind(apiEndPointsConfiguration);
            services.AddSingleton(apiEndPointsConfiguration);

            return services;
        }
    }
}
