using System;
using System.Text;

using CreditOne.Microservices.BuildingBlocks.MaskEncryptSensitiveData;

using Microsoft.Extensions.Logging;

namespace CreditOne.Microservices.BuildingBlocks.Logger.FileLogger
{
    /// <summary>
    /// Represents a type used to perform batching logger.
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
    ///  The constructor was modified adding a new parameter, LogLevel minLogLevel.
    ///  The method IsEnabled was modified to validates the _minLogLevel
    ///  </description>
    /// </item>
    /// </list>
    /// </remarks>
    public class BatchingLogger : ILogger
    {
        #region Private Members

        private readonly BatchingLoggerProvider _provider;
        private readonly string _category;

        #endregion

        #region Constructors

        public BatchingLogger(BatchingLoggerProvider loggerProvider, string categoryName)
        {
            _provider = loggerProvider;
            _category = categoryName;
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///  Begins a logical operation scope
        /// </summary>
        public IDisposable BeginScope<TState>(TState state)
        {
            return _provider.ScopeProvider?.Push(state);
        }

        /// <summary>
        /// Checks if the given logLevel is enabled.
        /// </summary>
        /// <param name="logLevel">Level to be checked.</param>
        public bool IsEnabled(LogLevel logLevel)
        {
            var minLogLevel = _provider.GetMinLogLevel(_category);

            return _provider.IsEnabled 
                && logLevel != LogLevel.None
                && minLogLevel != LogLevel.None
                && logLevel >= minLogLevel;
        }

        /// <summary>
        /// Writes a log entry
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="timestamp">Timestamp for the log entry</param>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a string message of the state and exception.</param>
        public void Log<TState>(DateTimeOffset timestamp, LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var builder = new StringBuilder();
            builder.Append(timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff zzz"));
            builder.Append(" [");
            builder.Append(logLevel.ToString());
            builder.Append("] ");
            builder.Append(_category);

            var scopeProvider = _provider.ScopeProvider;
            if (scopeProvider != null)
            {
                scopeProvider.ForEachScope((scope, stringBuilder) =>
                {
                    stringBuilder.Append(" => ").Append(scope);
                }, builder);

                builder.AppendLine(":");
            }
            else
            {
                builder.Append(": ");
            }

            builder.AppendLine(formatter(state, exception));

            if (exception != null)
            {
                while (exception != null)
                {
                    builder.AppendLine(exception.ToString());
                    exception = exception.InnerException;
                }
            }

            _provider.AddMessage(timestamp, builder.ToString().MaskAndEncryptSensitiveData());
        }

        /// <summary>
        /// Writes a log entry
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a string message of the state and exception.</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Log(DateTimeOffset.Now, logLevel, eventId, state, exception, formatter);
        }

        #endregion
    }
}