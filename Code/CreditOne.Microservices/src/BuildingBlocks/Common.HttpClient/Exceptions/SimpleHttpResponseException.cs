using System;
using System.Net;

namespace CreditOne.Microservices.BuildingBlocks.Common.HttpClient.Exceptions
{
    /// <summary>
    /// Represents a custom exception which provides the <c>HttpStatusCode</c> and the content as string
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
    ///		<term>4/13/2020</term>
    ///		<term>Luis Petitjean</term>
    ///		<term>RM-79</term>
    ///		<description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks> 	
    public class SimpleHttpResponseException : Exception
    {
        /// <summary>
        /// Response Status Code
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="statusCode">Response status code</param>
        /// <param name="content">Content</param>
        public SimpleHttpResponseException(HttpStatusCode statusCode,
                                           string content) : base(content)
        {
            StatusCode = statusCode;
        }
    }
}
