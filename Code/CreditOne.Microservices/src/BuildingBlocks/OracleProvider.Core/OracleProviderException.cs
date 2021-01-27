using System;
using System.Text;

using Oracle.ManagedDataAccess.Client;

namespace CreditOne.Microservices.BuildingBlocks.OracleProvider.Core
{
    public class OracleProviderException : Exception
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="command">Oracle command</param>
        public OracleProviderException(string message,
                                       OracleCommand command) : this(message, command, null)
        {
            _commandMessage = FormatCommand(command);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="command">Oracle command</param>
        /// <param name="innerException">Inner exception</param>
        public OracleProviderException(string message,
                                       OracleCommand command,
                                       Exception innerException) : base(message, innerException)
        {
            _commandMessage = FormatCommand(command);
        }

        #endregion

        #region Message Formatting

        private readonly string _commandMessage;

        /// <summary>
        /// Format the oracle exception.
        /// </summary>
        /// <returns>
        /// Formatted exception message
        /// </returns>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>04/05/2013</term>
        ///		<term>CTOWER</term>
        ///		<term>WO34415</term>
        ///		<description>Modifications to remove full card number from exception logging.</description>
        /// </item>
        /// </list>
        /// </remarks>
        private string FormatCommand(OracleCommand command)
        {
            if (command == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(command.CommandText);
            sb.AppendLine("(");

            foreach (OracleParameter parameter in command.Parameters)
            {
                sb.AppendFormat("\t{0} => {1}{2}",
                    parameter.ParameterName,
                    parameter.Value == null || parameter.Value == DBNull.Value ? "Null" :
                    parameter.Value.ToString(),
                    Environment.NewLine);
            }

            sb.AppendLine(")");
            sb.AppendLine();

            return sb.ToString();
        }

        #endregion

        #region Public methods

        public override string ToString()
        {
            return string.Format("{0}{1}{2}",
                Message + Environment.NewLine,
                _commandMessage,
                InnerException != null ? InnerException.ToString() : string.Empty);
        }

        #endregion
    }
}
