using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;

namespace CreditOne.Microservices.BuildingBlocks.Logger.FileLogger
{
    /// <summary>
    /// Extensions for adding the <see cref="FileLoggerProvider" /> to the <see cref="ILoggingBuilder" />
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///  <term>Date</term>
    ///  <term>Who</term>
    ///  <term>BR/WO</term>
    ///  <description>Description</description>
    /// </listheader>
    /// <item>
    ///  <term>05-12-2020</term>
    ///  <term>Daniel Lobaton</term>
    ///  <term>WO376694</term>
    ///  <description>
    ///  Migrated from CreditOne.Operations.Common
    ///  The extension method AddFile(this ILoggingBuilder builder) was modified to take the appsettings.json configuration.
    ///  </description>
    /// </item>
    /// </list>
    /// </remarks>
    public static class FileLoggerFactoryExtensions
    {
        /// <summary>
        /// Adds a file logger named 'File' to the factory.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
        public static ILoggingBuilder AddFile(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.AddSingleton<ILoggerProvider,FileLoggerProvider>();
            builder.Services.AddSingleton<IConfigureOptions<FileLoggerOptions>, FileLoggerOptionsSetup>();
            builder.Services.AddSingleton<IOptionsChangeTokenSource<FileLoggerOptions>, LoggerProviderOptionsChangeTokenSource<FileLoggerOptions, FileLoggerProvider>>();
            return builder;
        }

        /// <summary>
        /// Adds a file logger named 'File' to the factory.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
        /// <param name="filename">Sets the filename prefix to use for log files</param>
        public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string filename)
        {
            builder.AddFile(options => options.FileName = "log-");
            return builder;
        }

        /// <summary>
        /// Adds a file logger named 'File' to the factory.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
        /// <param name="configure">Configure an instance of the <see cref="FileLoggerOptions" /> to set logging options</param>
        public static ILoggingBuilder AddFile(this ILoggingBuilder builder, Action<FileLoggerOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }
            builder.AddFile();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}