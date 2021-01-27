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
    ///		<term>10/24/2019</term>
    ///		<term>Luis Petitjean</term>
    ///		<term>RM-47</term>
    ///		<description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks>
    public static class DbConfigurationServiceCollectionExtensions
    {
        /// <summary>
        ///  Creates the <c>DBConfiguration</c> service which contains a <c>Dictionary</c> of connection strings and
        ///  decrypts them based on Encryption:EncryptedData configuration value. Then it is added to the service collection.
        /// </summary>
        /// <param name="services">The services</param>
        /// <param name="configuration">The configurations</param>
        /// <returns>The service collection with the <c>DBConfiguration</c> service added which contains a <c>>Dictionary</c> of 
        /// clear text connection strings</returns>
        public static IServiceCollection AddDbConnectionStrings(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var dbConnectionStrings = new DbConnectionStrings();

            configuration.Bind(dbConnectionStrings);

            ConfigReader.ConfigReader.EncryptedData = configuration.GetValue<bool>("Encryption:EncryptedData");

            if (ConfigReader.ConfigReader.EncryptedData)
            {
                dbConnectionStrings.ConnectionStrings = ConfigReader.ConfigReader.Decrypt(dbConnectionStrings.ConnectionStrings);
            }

            services.AddSingleton(dbConnectionStrings);

            return services;
        }
    }
}
