using CreditOne.Microservices.BuildingBlocks.NotificationAgents;
using CreditOne.Microservices.BuildingBlocks.PerformanceMonitor.Configuration;
using System;
using System.Diagnostics;
using System.Threading;

namespace CreditOne.Microservices.BuildingBlocks.PerformanceMonitor.FDR
{
    /// <summary>
	/// This type handles alert conditions for the performance monitoring subsystem. It's job is to detect 
	/// failure conditions, and take the appropriate action (send an email, a network messages, etc.) to the
	/// appropriate person.
	/// </summary>
    public class FDRAlertHandler
    {
        #region Private Member

        private Timer _fdrTimeoutTimer;
        private Timer _fdrViewNotSupportedErrorsTimer;
        private Timer _fdrRequiredProgramNotFoundTimer;
        private Timer _fdrTransactionUnavailableExceptionTimer;
        private AlertHandlersConfiguration _alertHandlersConfig;
        private SMTPMailSender _smtpMailSender;
        private readonly IFDRMeasurement _measurement;

        #endregion

        #region Constructors and Destructor

        /// <summary>
        ///  Default Constructor
        /// </summary>
        public FDRAlertHandler(AlertHandlersConfiguration alertHandlersConfig, IFDRMeasurement measurement, SMTPMailSender smtpMailSender)
        {
            _alertHandlersConfig = alertHandlersConfig;
            _smtpMailSender = smtpMailSender;
            _measurement = measurement;
            SetupTimers();
        }

        /// <summary>
        /// DTOR disposes of timer resources.
        /// </summary>
        ~FDRAlertHandler()
        {
            if (_fdrTimeoutTimer != null)
                _fdrTimeoutTimer.Dispose();

            if (_fdrViewNotSupportedErrorsTimer != null)
                _fdrViewNotSupportedErrorsTimer.Dispose();

            if (_fdrRequiredProgramNotFoundTimer != null)
                _fdrRequiredProgramNotFoundTimer.Dispose();

            if (_fdrTransactionUnavailableExceptionTimer != null)
                _fdrTransactionUnavailableExceptionTimer.Dispose();

        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the FDR Timeout timer. When this timer fires, we check the number of timeouts that have
        /// occurred in the previous period. If they exceed a certain value, we send a notification email to
        /// the appropriate victim.
        /// </summary>
        /// <param name="ignoreThis">We just don't care about this.</param>
        public void OnCheckFDRTimeouts(object ignoreThis)
        {
            CheckAlertThreshold(_alertHandlersConfig.FDRTimeoutHandler, _measurement.TotalFDRTimeouts);
        }

        /// <summary>
        /// Handles the FDR View Not Supported Errors timer. When this timer fires, we check the number of FDR
        /// View Not Supported Errors that have occurred in the previous period. If they exceed a certain value,
        /// we send a notification email to the appropriate victim.
        /// </summary>
        /// <param name="ignoreThis">We just don't care about this.</param>
        public void OnCheckFdrViewNotSupportedErrors(object ignoreThis)
        {
            CheckAlertThreshold(_alertHandlersConfig.FdrViewNotSupportedErrorHandler, _measurement.TotalFdrViewNotSupportedErrors);
        }

        /// <summary>
        /// Handles the FDR Required Program Not Located Errors timer. When this timer fires, we check the number of FDR
        /// Required Program Not Located Errors that have occurred in the previous period. If they exceed a certain value,
        /// we send a notification email to the appropriate victim.
        /// </summary>
        /// <param name="ignoreThis">We just don't care about this.</param>
        public void OnCheckFdrRequiredProgramNotLocatedErrors(object ignoreThis)
        {
            CheckAlertThreshold(_alertHandlersConfig.FdrRequiredProgramNotFoundErrorHandler, _measurement.TotalFdrRequiredProgramNotLocatedErrors);
        }

        /// <summary>
        /// Handles the OnCheckFdrTransactionUnavailableException timer. When this timer fires, we check the number of
        /// exceptions that have occurred in the previous period. If they exceed a certain value, we send a notification
        /// email to the appropriate victim.
        /// </summary>
        /// <param name="ignoreThis">We just don't care about this.</param>
        public void OnCheckFdrTransactionUnavailableException(object ignoreThis)
        {
            CheckAlertThreshold(_alertHandlersConfig.FdrTransactionUnavailableExceptionHandler, _measurement.TotalFdrTransactionUnAvailableErrors);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Set up any required timers. Timers will only be spun up for alerts marked as active in the 
        /// configuration file.
        /// </summary>
        private void SetupTimers()
        {
            _fdrTimeoutTimer = null;
            if (_alertHandlersConfig.FDRTimeoutHandler.IsActive)
            {
                _fdrTimeoutTimer =
                    new Timer(new TimerCallback(OnCheckFDRTimeouts),
                                                null,
                                                _alertHandlersConfig.FDRTimeoutHandler.ThresholdMinutes * 60000,
                                                _alertHandlersConfig.FDRTimeoutHandler.ThresholdMinutes * 60000);
            }

            _fdrViewNotSupportedErrorsTimer = null;
            if (_alertHandlersConfig.FdrViewNotSupportedErrorHandler.IsActive)
            {
                _fdrViewNotSupportedErrorsTimer =
                    new Timer(new TimerCallback(OnCheckFdrViewNotSupportedErrors),
                                                null,
                                                _alertHandlersConfig.FdrViewNotSupportedErrorHandler.ThresholdMinutes * 60000,
                                                _alertHandlersConfig.FdrViewNotSupportedErrorHandler.ThresholdMinutes * 60000);
            }

            _fdrRequiredProgramNotFoundTimer = null;
            if (_alertHandlersConfig.FdrRequiredProgramNotFoundErrorHandler.IsActive)
            {
                _fdrRequiredProgramNotFoundTimer =
                    new Timer(new TimerCallback(OnCheckFdrRequiredProgramNotLocatedErrors),
                                                null,
                                                _alertHandlersConfig.FdrRequiredProgramNotFoundErrorHandler.ThresholdMinutes * 60000,
                                                _alertHandlersConfig.FdrRequiredProgramNotFoundErrorHandler.ThresholdMinutes * 60000);
            }

            _fdrTransactionUnavailableExceptionTimer = null;
            if (_alertHandlersConfig.FdrTransactionUnavailableExceptionHandler.IsActive)
            {
                _fdrTransactionUnavailableExceptionTimer = new Timer(new TimerCallback(OnCheckFdrTransactionUnavailableException), null,
                    _alertHandlersConfig.FdrTransactionUnavailableExceptionHandler.ThresholdMinutes * 60000,
                    _alertHandlersConfig.FdrTransactionUnavailableExceptionHandler.ThresholdMinutes * 60000);
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
