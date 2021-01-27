using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using CreditOne.Microservices.BuildingBlocks.Common.TokenProvider.Interfaces;
using CreditOne.Microservices.BuildingBlocks.Common.TokenProvider.Models;

using Newtonsoft.Json;

namespace CreditOne.Microservices.BuildingBlocks.Common.TokenProvider
{
    /// <summary>
    /// Api Gateway Token Provider for Token Provider Building Block.
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///  <term>Date</term>
    ///  <term>Who</term>
    ///  <term>BR/WO</term>
    ///  <description>Description</description>
    /// </listheader>
    /// <item>
    ///  <term>06-15-2020</term>
    ///  <term>Favio Massarini</term>
    ///  <term>MK-1001</term>
    ///  <description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks>
    public class ApiGatewayTokenProvider : IApiGatewayTokenProvider
    {
        #region Private Members

        private readonly HttpClient _client;

        private readonly ApiGatewayTokenProviderConfiguration _apiGatewayTokenProviderConfiguration;

        #endregion Private Members

        #region Constructor

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="apiGatewayTokenProviderConfiguration">Authentication parameters.</param>
        public ApiGatewayTokenProvider(HttpClient client, ApiGatewayTokenProviderConfiguration apiGatewayTokenProviderConfiguration)
        {
            this._client = client;
            this._apiGatewayTokenProviderConfiguration = apiGatewayTokenProviderConfiguration;
        }

        #endregion Constructor

        #region Public Methods

        /// <inheritdoc />
        public async Task<Token> GetToken()
        {
            var requestForm = new Dictionary<string, string>
                    {
                        {"grant_type", _apiGatewayTokenProviderConfiguration.GrantType },
                        {"client_id", _apiGatewayTokenProviderConfiguration.ClientId },
                        {"client_secret", _apiGatewayTokenProviderConfiguration.ClientSecret },
                    };

            HttpResponseMessage response = await _client.PostAsync(_apiGatewayTokenProviderConfiguration.AuthUri, new FormUrlEncodedContent(requestForm)).ConfigureAwait(false);
            var jsonContent = await response.Content.ReadAsStringAsync();
            Token token = JsonConvert.DeserializeObject<Token>(jsonContent);
            return token;
        }

        #endregion Public Methods
    }
}