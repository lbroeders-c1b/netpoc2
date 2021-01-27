using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CreditOne.Microservices.BuildingBlocks.Common.HttpClient
{
    /// <summary>
    /// Represents the HTTP Request Builder class
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
    ///     <term>7/12/2019</term>
    ///     <term>Mauro Allende</term>
    ///     <term>RM-47</term>
    ///     <description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks>
    public class HttpRequestBuilder
    {
        #region Private Fields

        private HttpMethod method = null;
        private string requestUri = "";
        private HttpContent content = null;
        private string bearerToken = "";
        private string acceptHeader = "application/json";
        private TimeSpan timeout = new TimeSpan(0, 0, 15);
        private bool allowAutoRedirect = false;

        #endregion

        #region Constructor

        public HttpRequestBuilder()
        {
        }

        #endregion

        #region Public Methods

        public HttpRequestBuilder AddMethod(HttpMethod method)
        {
            this.method = method;
            return this;
        }

        public HttpRequestBuilder AddRequestUri(string requestUri)
        {
            this.requestUri = requestUri;
            return this;
        }

        public HttpRequestBuilder AddContent(HttpContent content)
        {
            this.content = content;
            return this;
        }

        public HttpRequestBuilder AddBearerToken(string bearerToken)
        {
            var index = bearerToken.IndexOf("bearer ", StringComparison.InvariantCultureIgnoreCase);
            this.bearerToken = index >= 0 ? bearerToken.Substring(7) : bearerToken;
            return this;
        }

        public HttpRequestBuilder AddAcceptHeader(string acceptHeader)
        {
            this.acceptHeader = acceptHeader;
            return this;
        }

        public HttpRequestBuilder AddTimeout(TimeSpan timeout)
        {
            this.timeout = timeout;
            return this;
        }

        public HttpRequestBuilder AddAllowAutoRedirect(bool allowAutoRedirect)
        {
            this.allowAutoRedirect = allowAutoRedirect;
            return this;
        }

        public async Task<HttpResponseMessage> SendAsync()
        {
            // Check required arguments
            EnsureArguments();

            // Set up request
            var request = new HttpRequestMessage
            {
                Method = this.method,
                RequestUri = new Uri(this.requestUri)
            };

            if (this.content != null)
                request.Content = this.content;

            if (!string.IsNullOrEmpty(this.bearerToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.bearerToken);

            request.Headers.Accept.Clear();
            if (!string.IsNullOrEmpty(this.acceptHeader))
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(this.acceptHeader));

            // Setup client
            var handler = new HttpClientHandler();
            handler.AllowAutoRedirect = this.allowAutoRedirect;

            var client = new System.Net.Http.HttpClient(handler);
            client.Timeout = this.timeout;

            return await client.SendAsync(request);
        }

        #endregion

        #region Private Methods

        private void EnsureArguments()
        {
            if (this.method == null)
                throw new ArgumentNullException("Method");

            if (string.IsNullOrEmpty(this.requestUri))
                throw new ArgumentNullException("Request Uri");
        }

        #endregion
    }
}
