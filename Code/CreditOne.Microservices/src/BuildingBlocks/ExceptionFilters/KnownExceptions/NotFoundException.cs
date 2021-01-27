using System;
using System.Runtime.Serialization;

namespace CreditOne.Microservices.BuildingBlocks.ExceptionFilters.KnownExceptions
{

    public class NotFoundException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NotFoundException()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        public NotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info">Info</param>
        /// <param name="context">Context</param>
        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
