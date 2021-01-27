using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using CreditOne.Microservices.Account.Domain;
using CreditOne.Microservices.Account.Domain.Test;
using CreditOne.Microservices.Account.IDataProvider;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Newtonsoft.Json;

namespace CreditOne.Microservices.Account.API.Test
{
    /// <summary>
    /// Implements the unit tests for primary card holder controller class
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
    ///		<term>5/31/2019</term>
    ///		<term>Christian Azula</term>
    ///		<term>RM-47</term>
    ///		<description>Initial implementation</description>  
    ///	</item>
    /// <item>
    ///		<term>8/21/2020</term>
    ///		<term>Mauro Allende</term>
    ///		<term>RM-79</term>
    ///		<description>Refactor unit tests</description>  
    /// </item>
    /// </list>
    /// </remarks> 
    [TestClass]
    public class PrimaryCardholderControllerTest
    {
        #region Private Constants

        private const string PATHGetByCreditAccountId = "api/v1/creditaccounts/{0}/primary-cardholder";

        #endregion

        #region Private Members

        private readonly Mock<IPrimaryCardholderDataProvider> _primaryCardholderDataProviderMock;
        private readonly PrimaryCardholderStub _primaryCardholderStub;

        private TestServer _server;
        private HttpClient _client;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>      
        public PrimaryCardholderControllerTest()
        {
            _primaryCardholderDataProviderMock = new Mock<IPrimaryCardholderDataProvider>();
            _primaryCardholderStub = new PrimaryCardholderStub();

            CreateServer();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The test is Ok if one Primary Cardholder is found
        /// </summary>
        [TestMethod]
        public async Task GetByCreditAccountIdShouldReturnOkWithData()
        {
            // Arrange
            _primaryCardholderDataProviderMock.Setup(x => x.GetByCreditAccountId(It.IsAny<decimal>()))
                .ReturnsAsync(_primaryCardholderStub.GetPrimaryCardholders());

            // Act
            var response = await _client.GetAsync(string.Format(PATHGetByCreditAccountId, It.IsAny<decimal>()));
            var result = JsonConvert.DeserializeObject<PrimaryCardholder>(response.Content.ReadAsStringAsync().Result);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsInstanceOfType(result, typeof(PrimaryCardholder));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Create Test Server
        /// </summary>      
        private void CreateServer()
        {
            _server = new TestServer(new WebHostBuilder()
               .UseStartup<StartupTest>()
               .ConfigureTestServices(services =>
               {
                   services.AddSingleton(_primaryCardholderDataProviderMock.Object);
               }));

            _client = _server.CreateClient();
        }

        #endregion
    }
}