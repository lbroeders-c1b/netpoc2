using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using CreditOne.Microservices.ApiClients.Account.Interfaces;
using CreditOne.Microservices.ApiClients.Account.Models.Response;
using CreditOne.Microservices.BuildingBlocks.BearerTokenProvider.Interfaces;
using CreditOne.Microservices.BuildingBlocks.Common.Configuration;
using CreditOne.Microservices.BuildingBlocks.Common.HttpClient;

namespace CreditOne.Microservices.ApiClients.Account
{
    /// <summary>
    /// Implements the credit account service class
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
    ///     <term>2/18/2020</term>
    ///     <term>Luis Petitjean</term>
    ///     <term>RM-80</term>
    ///     <description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks>
    public class CreditAccountService : ICreditAccountService
    {
        #region Private Constants

        private const string AccountBaseUriKey = "AccountBaseUri";

        #endregion

        #region Private Members

        private readonly ApiEndPointsConfiguration _apiEndPointsConfiguration;

        /// <summary>
        /// A provider to get the authentication token to use in the request.
        /// </summary>
        private readonly string _bearerToken;

        /// <summary>
        /// The Base Uri for the client.
        /// </summary>
        private readonly string _accountBaseUri;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiEndPointsConfiguration">Api endpoints configuration</param>
        /// <param name="tokenProvider">Token provider</param>
        public CreditAccountService(ApiEndPointsConfiguration apiEndPointsConfiguration,
                                    ITokenProvider tokenProvider)
        {
            _bearerToken = tokenProvider.GetToken();
            _apiEndPointsConfiguration = apiEndPointsConfiguration;
            _accountBaseUri = _apiEndPointsConfiguration.Uris[AccountBaseUriKey].Uri.AbsoluteUri;
        }

        #endregion

        #region Public Methods     

        /// Retrieves a credit accounts for a given credit account identifier
        /// </summary>
        /// <param name="CreditAccountId">Credit account identifier</param>
        /// <returns>Returns the credit account data for <c>ResponseAccount.CreditAccount</c></returns>
        public async Task<CreditAccountResponse> GetAsync(decimal id)
        {
            var requestUri = $"{_accountBaseUri}/{id}";

            var response = await HttpRequestFactory.Get(requestUri, _bearerToken);

            return HttpResponseHandler.Parse<CreditAccountResponse>(response, HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Gets the credit account's primary cardholder resource
        /// </summary>
        /// <param name="creditAccountId">Credit account identifier</param>
        /// <returns>The <see cref="PrimaryCardholder"/></returns>
        public async Task<PrimaryCardholderResponse> GetPrimaryCardholderModelAsync(decimal creditAccountId)
        {
            var requestUri = $"{_accountBaseUri}/{creditAccountId}/primary-cardholder";

            var response = await HttpRequestFactory.Get(requestUri, _bearerToken);

            return HttpResponseHandler.Parse<PrimaryCardholderResponse>(response);
        }

        /// <summary>
        /// Retrieves a credit accounts for a given customer identifier
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>List of credit accounts</returns>
        public async Task<List<CreditAccountResponse>> GetByCustomerIdAsync(decimal customerId)
        {
            var requestUri = $"{_accountBaseUri}?customerId={customerId}";

            var response = await HttpRequestFactory.Get(requestUri, _bearerToken);

            return HttpResponseHandler.Parse<List<CreditAccountResponse>>(response, HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Gets the credit account's cardholders resource
        /// </summary>
        /// <param name="creditAccountId">The credit account identifier</param>
        /// <returns>List of cardholders</returns>
        public async Task<List<CardholderBaseResponse>> GetCardholdersAsync(decimal creditAccountId)
        {
            var requestUri = $"{_accountBaseUri}/{creditAccountId}/cardholders";

            var response = await HttpRequestFactory.Get(requestUri, _bearerToken);

            return HttpResponseHandler.Parse<List<CardholderBaseResponse>>(response);
        }

        /// <summary>
        /// Retrieves the emails for a given credit account identifier
        /// </summary>
        /// <param name="creditAccountId">Credit account identifier</param>
        /// <returns>List of emails</returns>
        public async Task<List<CreditAccountEmailResponse>> GetEmailsAsync(decimal creditAccountId)
        {
            var requestUri = $"{_accountBaseUri}/{creditAccountId}/emails";

            var response = await HttpRequestFactory.Get(requestUri, _bearerToken);

            return HttpResponseHandler.Parse<List<CreditAccountEmailResponse>>(response);
        }

        #endregion
    }
}
