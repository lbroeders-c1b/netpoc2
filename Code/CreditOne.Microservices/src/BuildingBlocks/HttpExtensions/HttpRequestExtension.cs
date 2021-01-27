using System.IO;
using System.Text;

using Microsoft.AspNetCore.Http;

namespace CreditOne.Microservices.BuildingBlocks.HttpExtensions
{
    /// <summary>
    /// Represents the http request extension
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
    ///     <term>4/03/2020</term>
    ///     <term>Christian Azula</term>
    ///     <term>RM-80</term>
    ///     <description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks>
    public static class HttpRequestExtension
    {
        /// <summary>
        /// Sets request message 
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        public static string FormatRequest(this HttpRequest httpRequest)
        {
            var request = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.Path}";

            if (httpRequest.QueryString.HasValue)
            {
                request += $" QueryString: {httpRequest.QueryString.Value}";
            }

            if (httpRequest.Body.CanSeek)
            {
                string bodyAsString;

                httpRequest.Body.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(httpRequest.Body, Encoding.UTF8))
                {
                    bodyAsString = reader.ReadToEnd();
                }

                request += $" Body: {bodyAsString}";
            }

            return request;
        }
    }
}