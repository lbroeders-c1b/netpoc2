using System;

using CreditOne.Microservices.BuildingBlocks.NotificationAgents;
using CreditOne.Microservices.BuildingBlocks.PerformanceMonitor.Configuration;

namespace CreditOne.Microservices.BuildingBlocks.PerformanceMonitor.Base
{
    /// <summary>
    /// Represents the base for all performance alert handlers
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
	///		<term>11/01/2019</term>
	///		<term>Luis Petitjean</term>
	///		<term>RM-47</term>
	///		<term>Refactored from the <c>FNBM.PerformanceMonitor.AlertHandler</c></term>
	/// </item>
	/// </list>
	/// </remarks>
    public abstract class AlertBaseHandler : IDisposable
    {
        private readonly SmtpMailSender _smtpMailSender;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="smtpMailSender">The Smtp to send the alerts</param>
        public AlertBaseHandler(SmtpMailSender smtpMailSender)
        {
            _smtpMailSender = smtpMailSender;
        }

        /// <summary>
        /// Checks a perfomance counter went over the set threshold and sends an alert email 
        /// </summary>
        /// <param name="alertHandlerParameters">The performance counter threshold and the email parameters</param>
        /// <param name="counterRawValue">The performance counter raw value</param>
        protected void CheckAlertThreshold(AlertHandlerParameters alertHandlerParameters, long counterRawValue)
        {
            if (counterRawValue > alertHandlerParameters.ThresholdCount)
            {
                _smtpMailSender.SendMessage(alertHandlerParameters.EmailTo,
                                            alertHandlerParameters.EmailFrom,
                                            $"[{Environment.MachineName}] {alertHandlerParameters.EmailSubject}",
                                            string.Format(alertHandlerParameters.EmailBody, counterRawValue, alertHandlerParameters.ThresholdMinutes, Environment.MachineName),
                                            alertHandlerParameters.SmtpServer);
            }
        }

        /// <summary>
        /// Dispose timers
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Set up any required timers. Timers will only be spun up for alerts marked as active in the 
        /// configuration file.
        /// </summary>
        protected abstract void SetupTimers();             
    }
}