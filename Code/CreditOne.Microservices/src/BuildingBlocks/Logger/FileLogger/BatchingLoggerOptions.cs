using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace CreditOne.Microservices.BuildingBlocks.Logger.FileLogger
{
    /// <summary>
    /// Represent the batching logger configuration.
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
    ///  ApplicationName and LogLevels properties were added
    ///  </description>
    /// </item>
    /// </list>
    /// </remarks>
    public class BatchingLoggerOptions
    {
        #region Private Members

        private int? _batchSize;
        private int? _backgroundQueueSize = 1000;
        private TimeSpan _flushPeriod = TimeSpan.FromSeconds(1);

        #endregion

        #region Public Members

        public string ApplicationName { get; set; }

        public Dictionary<string, LogLevel> LogLevel { get; set; }

        /// <summary>
        /// Gets or sets the period after which logs will be flushed to the store.
        /// </summary>
        public TimeSpan FlushPeriod
        {
            get { return _flushPeriod; }
            set
            {
                if (value <= TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(FlushPeriod)} must be positive.");
                }
                _flushPeriod = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum size of the background log message queue or null for no limit.
        /// After maximum queue size is reached log event sink would start blocking.
        /// Defaults to <c>1000</c>.
        /// </summary>
        public int? BackgroundQueueSize
        {
            get { return _backgroundQueueSize; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(BackgroundQueueSize)} must be non-negative.");
                }
                _backgroundQueueSize = value;
            }
        }

        /// <summary>
        /// Gets or sets a maximum number of events to include in a single batch or null for no limit.
        /// </summary>
        /// Defaults to <c>null</c>.
        public int? BatchSize
        {
            get { return _batchSize; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(BatchSize)} must be positive.");
                }
                _batchSize = value;
            }
        }

        /// <summary>
        /// Gets or sets value indicating if logger accepts and queues writes.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether scopes should be included in the message.
        /// Defaults to <c>false</c>.
        /// </summary>
        public bool IncludeScopes { get; set; } = false;

        #endregion
    }
}