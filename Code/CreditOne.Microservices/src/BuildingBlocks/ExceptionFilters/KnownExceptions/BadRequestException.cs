using System;
using System.Runtime.Serialization;

namespace CreditOne.Microservices.BuildingBlocks.ExceptionFilters.KnownExceptions
{
    /// <summary>
    /// Represents the knwon bad request exception class
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///     <term>Date</term>
    ///     <term>Who</term>
    ///     <term>BR/WO</term>
    ///     <description>Description</description>
    /// </listheader>
    /// <item>
    ///     <term>4/16/2020</term>
    ///     <term>Luis Petitjean</term>
    ///     <term>RM-79</term>
    ///     <description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks>
    public class BadRequestException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BadRequestException()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        public BadRequestException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info">Info</param>
        /// <param name="context">Context</param>
        protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
