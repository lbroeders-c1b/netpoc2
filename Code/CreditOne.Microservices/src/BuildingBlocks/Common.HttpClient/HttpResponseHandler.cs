using System.Net.Http;

using CreditOne.Microservices.BuildingBlocks.Common.HttpClient.Exceptions;

namespace CreditOne.Microservices.BuildingBlocks.Common.HttpClient
{
    /// <summary>
    /// Represents the HTTP Response Handler class
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
    public static class HttpResponseHandler
    {
        /// <summary>
        /// Parses the given response. Handles <c>HttpStatusCode</c> optional values to return the default T value 
        /// instead of throwing an exception.Other responses out of the 200-299 status code range 
        /// throws <c>SimpleHttpResponseException</c>
        /// </summary>
        /// <typeparam name="T">The type the content result deserializes to</typeparam>
        /// <param name="reponse">The response from the request</param>
        /// <returns>A T type instance or null. Otherwise throws <c>SimpleHttpResponseException</c></returns>
        public static T Parse<T>(HttpResponseMessage response,
                                 params System.Net.HttpStatusCode[] statusCodes)
        {
            if (!response.IsSuccessStatusCode)
            {
                for (var i = 0; i < statusCodes.Length; i++)
                {
                    if (response.StatusCode == statusCodes[i])
                    {
                        return default(T);
                    }
                }

                throw new SimpleHttpResponseException(response.StatusCode, response.ContentAsString());
            }

            return response.ContentAsType<T>();
        }

    }
}
