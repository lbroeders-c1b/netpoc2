using CreditOne.Microservices.BuildingBlocks.NotificationAgents;
using CreditOne.Microservices.BuildingBlocks.PerformanceMonitor.Base;
using CreditOne.Microservices.BuildingBlocks.PerformanceMonitor.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;

namespace CreditOne.Microservices.BuildingBlocks.PerformanceMonitor.Database
{
    /// <summary>
    /// This type handles alert conditions for the performance monitoring subsystem. It's job is to detect 
    /// failure conditions, and take the appropriate action (send an email, a network messages, etc.) to the
    /// appropriate person.
    /// </summary>
    public class DatabaseAlertHandler : AlertBaseHandler
    {
        #region Private Member   

        private Timer _dbOpenFailureTimer;
        private Timer _databaseOperationTimeThresholdExceededTimer;
        private DatabaseAlertHandlerParametersConfiguration _alertHandlersConfig;
        private readonly ILogger _logger;
        private readonly IDatabaseMeasurement _measurement;

        #endregion

        #region Constructors and Destructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options">Alert Handlers Config</param>
        /// <param name="measurement">Measurement</param>
        /// <param name="smtpMailSender">Smtp Provider</param>
        /// <param name="logger">Logger</param>
        public DatabaseAlertHandler(IOptionsMonitor<DatabaseAlertHandlerParametersConfiguration> options, 
                                    IDatabaseMeasurement measurement, 
                                    SmtpMailSender smtpMailSender, 
                                    ILogger<DatabaseAlertHandler> logger) : base(smtpMailSender)
        {
            _measurement = measurement;
            _logger = logger;
            _alertHandlersConfig = options.CurrentValue;
            SetupTimers();

            options.OnChange(OnConfigurationChange);
        }

        public override void Dispose()
        {
            if (_dbOpenFailureTimer != null)
            {
                _dbOpenFailureTimer.Dispose();
            }
            if (_databaseOperationTimeThresholdExceededTimer != null)
            {
                _databaseOperationTimeThresholdExceededTimer.Dispose();
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the DB Open Failures timer. When this timer fires, we check the number of failures that have
        /// occurred in the previous period. If they exceed a certain value, we send a notification email to
        /// the appropriate victim.
        /// </summary>
        /// <param name="ignoreThis">We just don't care about this.</param>
        public void OnCheckDBOpenFailures(object ignoreThis)
        {
            try
            {
                base.CheckAlertThreshold(_alertHandlersConfig.DatabaseOpenFailureHandler, _measurement.TotalDatabaseOpenFailures.RawValue);
                _measurement.TotalDatabaseOpenFailures.RawValue = 0;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "OracleAlertHandler - OnCheckDBOpenFailures - Failed check alert threshold");
            }
        }

        /// <summary>
        /// Handles the DatabaseOperationTimeThresholdExceeded timer. When this timer fires, we check the number of
        /// timeouts that have occurred in the previous period. If they exceed a certain value, we send a notification
        /// email to the appropriate victim.
        /// </summary>
        /// <param name="ignoreThis">We just don't care about this.</param>
        public void OnCheckDatabaseOperationTimeThresholdExceeded(object ignoreThis)
        {
            try
            {
                base.CheckAlertThreshold(_alertHandlersConfig.DatabaseOperationTimeThresholdHandler, _measurement.DatabaseOperationTimeThresholdExceededCount.RawValue);
                _measurement.DatabaseOperationTimeThresholdExceededCount.RawValue = 0;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "OracleAlertHandler - OnCheckDatabaseOperationTimeThresholdExceeded - Failed check alert threshold");
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Set up any required timers. Timers will only be spun up for alerts marked as active in the 
        /// configuration file.
        /// </summary>
        protected override void SetupTimers()
        {
            _dbOpenFailureTimer = null;
            if (_alertHandlersConfig.DatabaseOpenFailureHandler.IsActive)
            {
                _dbOpenFailureTimer =
                    new Timer(new TimerCallback(OnCheckDBOpenFailures),
                                                null,
                                                _alertHandlersConfig.DatabaseOpenFailureHandler.ThresholdMinutes * 60000,
                                                _alertHandlersConfig.DatabaseOpenFailureHandler.ThresholdMinutes * 60000);
            }

            _databaseOperationTimeThresholdExceededTimer = null;
            if (_alertHandlersConfig.DatabaseOperationTimeThresholdHandler.IsActive)
            {
                _databaseOperationTimeThresholdExceededTimer =
                    new Timer(new TimerCallback(OnCheckDatabaseOperationTimeThresholdExceeded),
                                                null,
                                                _alertHandlersConfig.DatabaseOperationTimeThresholdHandler.ThresholdMinutes * 60000,
                                                _alertHandlersConfig.DatabaseOperationTimeThresholdHandler.ThresholdMinutes * 60000);
            }
        }

        private void OnConfigurationChange(DatabaseAlertHandlerParametersConfiguration options)
        {
            _alertHandlersConfig = options;
            SetupTimers();
        }

        #endregion
    }
}
