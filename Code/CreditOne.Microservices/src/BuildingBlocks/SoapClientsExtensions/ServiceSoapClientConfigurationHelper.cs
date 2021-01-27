using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;

namespace CreditOne.Microservices.BuildingBlocks.SoapClientsExtensions
{
    /// <summary>
    /// Implements binding and endpoint configuration for Soap Services Clients
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
    ///		<term>7/15/2020</term>
    ///		<term>Juan Blanco</term>
    ///		<term>RM-79</term>
    ///		<description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks> 	
    public class ServiceSoapClientConfigurationHelper
    {
        #region Private Constants

        private const string Https = "https://";
        private const string Http = "http://";
        private const string EndPointUriFormatExceptionMessage = "The endpoint URL must start with https:// or http://.";

        #endregion

        #region Public Methods

        /// <summary>
        /// Instantiates a <c>BasicHttpBinding</c> object by a given URL
        /// </summary>
        /// <param name="remoteAddress">Remote address</param>
        /// <returns>A <c>BasicHttpBinding</c> instance</returns>
        public static Binding GetBindingForEndpoint(string remoteAddress)
        {
            BasicHttpBinding httpsBinding = remoteAddress.StartsWith(Https) ?
                new BasicHttpBinding(BasicHttpSecurityMode.Transport) :
                new BasicHttpBinding();

            var integerMaxValue = int.MaxValue;
            httpsBinding.MaxBufferSize = integerMaxValue;
            httpsBinding.MaxReceivedMessageSize = integerMaxValue;
            httpsBinding.ReaderQuotas = XmlDictionaryReaderQuotas.Max;
            httpsBinding.AllowCookies = true;

            return httpsBinding;
        }

        /// <summary>
        /// Instantiates a <c>EndpointAddress</c> object by a given URL
        /// </summary>
        /// <param name="endpointUrl">Remote address</param>
        /// <returns>A <c>EndpointAddress</c> instance</returns>
        public static EndpointAddress GetEndpointAddress(string endpointUrl)
        {
            if (!endpointUrl.StartsWith(Https) && !endpointUrl.StartsWith(Http))
            {
                throw new UriFormatException(EndPointUriFormatExceptionMessage);
            }

            return new EndpointAddress(endpointUrl);
        }

        #endregion
    }
}
