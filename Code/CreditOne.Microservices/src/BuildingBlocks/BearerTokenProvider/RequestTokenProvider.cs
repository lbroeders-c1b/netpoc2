using CreditOne.Microservices.BuildingBlocks.BearerTokenProvider.Interfaces;

using Microsoft.AspNetCore.Http;

namespace CreditOne.Microservices.BuildingBlocks.BearerTokenProvider
{
    public class RequestTokenProvider : ITokenProvider
    {
        #region Private Members

        private readonly IHttpContextAccessor _httpContextAccessor;

        private const string AuthorizationKey = "Authorization";

        #endregion Private Members

        #region Constructor

        /// <summary>
        /// Constructor for Token Provider
        /// </summary>
        /// <param name="httpContextAccessor">Http Context Accessor</param>
        public RequestTokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        #endregion Constructor

        /// <summary>
        /// Gets the authentication token from the HTTP Request header.
        /// </summary>
        /// <returns>The bearer token.</returns>
        public string GetToken()
        {
            return this._httpContextAccessor.HttpContext.Request.Headers[AuthorizationKey];
        }
    }
}
