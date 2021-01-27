using CreditOne.Microservices.BuildingBlocks.NotificationAgents;
using CreditOne.Microservices.BuildingBlocks.PerformanceMonitor.Configuration;
using System;
using System.Diagnostics;
using System.Threading;

namespace CreditOne.Microservices.BuildingBlocks.PerformanceMonitor.Cache
{
    /// <summary>
	/// This type handles alert conditions for the performance monitoring subsystem. It's job is to detect 
	/// failure conditions, and take the appropriate action (send an email, a network messages, etc.) to the
	/// appropriate person.
	/// </summary>
    public class CacheAlertHandler
    {
        #region Private Member

        private Timer _cachedProviderExceptionsTimer;
        private AlertHandlersConfiguration _alertHandlersConfig;
        private SmtpMailSender _smtpMailSender;
        private readonly ICacheMeasurement _measurement;

        #endregion

        #region Constructors and Destructor

        /// <summary>
        ///  Default Constructor
        /// </summary>
        public CacheAlertHandler(AlertHandlersConfiguration alertHandlersConfig, ICacheMeasurement measurement, SmtpMailSender smtpMailSender)
        {
            _alertHandlersConfig = alertHandlersConfig;
            _smtpMailSender = smtpMailSender;
            _measurement = measurement;
            SetupTimers();
        }

        /// <summary>
        /// DTOR disposes of timer resources.
        /// </summary>
        ~CacheAlertHandler()
        {
            if (_cachedProviderExceptionsTimer != null)
                _cachedProviderExceptionsTimer.Dispose();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the OnCheckCachedDataProviderExceptionCountThresholdExceeded timer. When this timer fires, we check the number of
        /// exceptions that have occurred in the previous period. If they exceed a certain value, we send a notification
        /// email to the appropriate victim.
        /// </summary>
        /// <param name="ignoreThis">We just don't care about this.</param>
        public void OnCheckCachedDataProviderExceptionCountThresholdExceeded(object ignoreThis)
        {
            CheckAlertThreshold(_alertHandlersConfig.CachedDataProviderExceptionHandler, _measurement.CachedDataProviderExceptionsCount);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Set up any required timers. Timers will only be spun up for alerts marked as active in the 
        /// configuration file.
        /// </summary>
        private void SetupTimers()
        {
            _cachedProviderExceptionsTimer = null;
            if (_alertHandlersConfig.CachedDataProviderExceptionHandler.IsActive)
            {
                _cachedProviderExceptionsTimer =
                    new Timer(new TimerCallback(OnCheckCachedDataProviderExceptionCountThresholdExceeded),
                                                null,
                                                _alertHandlersConfig.CachedDataProviderExceptionHandler.ThresholdMinutes * 60000,
                                                _alertHandlersConfig.CachedDataProviderExceptionHandler.ThresholdMinutes * 60000);
            }
        }

        private void CheckAlertThreshold(AlertHandlerParameters alertHandlerParameters, PerformanceCounter counter)
        {
            if (counter.RawValue > alertHandlerParameters.ThresholdCount)
            {
                _smtpMailSender.SendMessage(alertHandlerParameters.EmailTo,
                                            alertHandlerParameters.EmailFrom,
                                            alertHandlerParameters.EmailSubject,
                                            string.Format(alertHandlerParameters.EmailBody, counter.RawValue, alertHandlerParameters.ThresholdMinutes, Environment.MachineName),
                                            alertHandlerParameters.SmtpServer);
            }

            counter.RawValue = 0;
        }

        #endregion
    }
}
