namespace CreditOne.Microservices.BuildingBlocks.PerformanceMonitor.Configuration
{
    /// <summary>
    /// A little wrapper class to contain configuration parameters.
    /// </summary>
    public class AlertHandlerParameters
    {
        /// <summary>
        /// If true, sets the handler active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The number of times the event must occur in 'thresholdMinutes' time.
        /// </summary>
        public int ThresholdCount { get; set; }

        /// <summary>
        /// The number of minutes that 'thresholdCount' events must occur.
        /// </summary>
        public int ThresholdMinutes { get; set; }

        /// <summary>
        /// The SMTP server to use to send email alerts.
        /// </summary>
        public string SmtpServer { get; set; }

        /// <summary>
        /// The email address(es) to send the alert to.
        /// </summary>
        public string EmailTo { get; set; }

        /// <summary>
        /// Who the email is from.
        /// </summary>
        public string EmailFrom { get; set; }

        /// <summary>
        /// The subject line of the email.
        /// </summary>
        public string EmailSubject { get; set; }

        /// <summary>
        /// The body of the email message.
        /// </summary>
        public string EmailBody { get; set; }
    }
}
