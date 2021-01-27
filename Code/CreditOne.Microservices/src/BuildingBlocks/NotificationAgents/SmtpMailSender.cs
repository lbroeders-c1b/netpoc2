using System.Net.Mail;
using System.Text.RegularExpressions;

namespace CreditOne.Microservices.BuildingBlocks.NotificationAgents
{
    /// <summary>
	///  Wraps an instance of the SmtpMail class. Delivers a failure notice via email.
	/// </summary>
    public class SmtpMailSender
    {
        #region Constructors

        /// <summary>
        /// Default CTOR does nothing.
        /// </summary>
        public SmtpMailSender()
        {

        }

        #endregion

        #region Public Member Methods

        /// <summary>
        /// Attempts to send the message. Client should have called ValidateParameters() first.
        /// </summary>
        /// <param name="to">The address to send email to.</param>
        /// <param name="from">The address the email is from.</param>
        /// <param name="subject">The subject line of the email.</param>
        /// <param name="body">The body of the email.</param>
        /// <param name="smtpServer">The SMTP server to use to send email.</param>
        /// <returns>True if send succeeded, false if not.</returns>
        public void SendMessage(string to, string from, string subject, string body, string smtpServer)
        {
            if (ValidateAddresses(to, from))
            {
                // Create the message
                MailMessage message = new MailMessage(from, to, subject, body);
                message.IsBodyHtml = false;
                message.Priority = MailPriority.High;

                // Send the mail
                new SmtpClient(smtpServer).Send(message);
            }
        }

        #endregion

        #region Private Member Methods

        /// <summary>
        /// Validate the parameters.
        /// </summary>
        /// <param name="to">The address to send email to.</param>
        /// <param name="from">The address the email is from.</param>
        /// <returns>True if all parameters are valid, false if not.</returns>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>07/23/2015</term>
        ///		<term>Rmeine</term>
        ///		<term>WO70237</term>
        ///		<description>Switched the split symbol from ; to , 
        ///		because system.net.mail smtp class requires comma on split</description>
        /// </item>
        /// </list>
        /// </remarks>
        private bool ValidateAddresses(string to, string from)
        {
            // Check the to parameter for valid form. We allow more than one email address, separated by the
            // comma character; But since our regular expression only checks one at a time, we have to
            // to split them up into separate addresses.
            // Note: Blatantly stole expression from MSDN docs.
            Regex properEmailAddressFormat = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            Match match;

            // Split the addresses into separate strings.
            string[] emailAddresses = to.Split(new char[] { ',' });

            // Loop and check each one.
            foreach (string ema in emailAddresses)
            {
                match = properEmailAddressFormat.Match(ema);
                if (!match.Success)
                    return false;
            }

            // Check the from parameter for valid form (only one email address allowed here.
            match = properEmailAddressFormat.Match(from);
            if (!match.Success)
                return false;

            return true;
        }

        #endregion
    }
}