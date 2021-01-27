using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CreditOne.Microservices.BuildingBlocks.Common.Domain
{
    /// <summary>
    /// Class for CustomExtensionMethods
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
    ///		<term>07-1-2019</term>
    ///		<term>Daniel Lobaton</term>
    ///		<term>RM-47</term>
    ///		<description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks>
    public static class CustomExtensionMethods
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            //{
            //    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

            //    var factory = new ConnectionFactory()
            //    {
            //        HostName = configuration["RabbitMQHostName"],
            //        DispatchConsumersAsync = true
            //    };

            //    if (!string.IsNullOrEmpty(configuration["RabbitMQUserName"]))
            //    {
            //        factory.UserName = configuration["RabbitMQUserName"];
            //    }

            //    if (!string.IsNullOrEmpty(configuration["RabbitMQPassword"]))
            //    {
            //        factory.Password = configuration["RabbitMQPassword"];
            //    }

            //    return new DefaultRabbitMQPersistentConnection(factory, logger, 5);
            //});

            return services;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            //{
            //    var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
            //    var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
            //    var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
            //    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

            //    return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, "Logging", 5);
            //});

            //services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return services;
        }
    }
}
