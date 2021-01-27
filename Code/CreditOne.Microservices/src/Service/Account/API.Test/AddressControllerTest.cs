using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using CreditOne.Microservices.Account.Domain.Test;
using CreditOne.Microservices.Account.IDataProvider;
using CreditOne.Microservices.Account.Models;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Newtonsoft.Json;

namespace CreditOne.Microservices.Account.API.Test
{
    /// <summary>
    /// Implements the unit test for address controller
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
    ///		<term>05-29-2019</term>
    ///		<term>Daniel Lobaton</term>
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
    public class AddressControllerTest
    {
        #region Private Constants

        private const string ExceptionMessage = "TestExceptionMessage";
        private const string PATHGetById = "api/v1/creditaccounts/{0}/addresses";

        #endregion

        #region Private Members

        private readonly Mock<IAddressDataProvider> _addressDataProviderMock;
        private readonly AddressStub _addressStub;
        private TestServer _server;
        private HttpClient _client;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>      
        public AddressControllerTest()
        {
            _addressDataProviderMock = new Mock<IAddressDataProvider>();
            _addressStub = new AddressStub();

            CreateServer();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The test is Ok if one or more addresses are found, the result is a list of addresses
        /// </summary>
        [TestMethod]
        public async Task GetAddressesByIdCreditAccountShouldReturnListAddresses()
        {
            // Arrange
            _addressDataProviderMock.Setup(x => x.GetByCreditAccountId(It.IsAny<decimal>())).ReturnsAsync(_addressStub.GetAll());

            // Act
            var response = await _client.GetAsync(string.Format(PATHGetById, It.IsAny<decimal>()));
            var result = JsonConvert.DeserializeObject<List<AddressModel>>(response.Content.ReadAsStringAsync().Result);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsInstanceOfType(result, typeof(List<AddressModel>));
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
                    services.AddSingleton(_addressDataProviderMock.Object);
                })
                );

            _client = _server.CreateClient();
        }

        #endregion
    }
}