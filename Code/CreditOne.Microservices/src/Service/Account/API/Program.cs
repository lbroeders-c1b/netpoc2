using System;

using CreditOne.Microservices.BuildingBlocks.Logger.FileLogger;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CreditOne.Microservices.Account.API
{
    /// <summary>
    /// Implements the program class
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
    ///		<term>5/27/2019</term>
    ///		<term>Armando Soto</term>
    ///		<term>RM-47</term>
    ///		<description>Initial implementation</description>
    ///	</item>
    /// </list>
    /// </remarks>
    public class Program
    {
        #region Constants

        private const string MicroserviceHasStarted = "Account microservice has started.";
        private const string MicroserviceHasStopped = "Account microservice has stopped unexpectedly!";

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the program execution
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            logger.LogInformation(MicroserviceHasStarted);

            try
            {
                host.Run();
            }
            catch (Exception exception)
            {
                logger.LogCritical(exception, MicroserviceHasStopped);
            }
        }

        /// <summary>
        /// Creates a web host builder
        /// </summary>
        /// <param name="args">Arguments</param>
        /// <returns>Web host builder</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
                 WebHost.CreateDefaultBuilder(args)
                     .UseStartup<Startup>()
                     .ConfigureLogging((hostingContext, logging) =>
                     {
                         logging.ClearProviders();
                         logging.AddConsole();
                         logging.AddFile();
                     });

        #endregion
    }
}