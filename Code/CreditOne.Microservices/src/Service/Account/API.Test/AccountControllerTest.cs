using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using CreditOne.Microservices.Account.Domain;
using CreditOne.Microservices.Account.Domain.Test;
using CreditOne.Microservices.Account.IDataProvider;
using CreditOne.Microservices.Account.Models;
using CreditOne.Microservices.Account.Service;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Newtonsoft.Json;

namespace CreditOne.Microservices.Account.API.Test
{
    /// <summary>
    /// Implements the unit test for account controller
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
    ///		<term>5/29/2019</term>
    ///		<term>arsoto</term>
    ///		<term>RM-47</term>
    ///		<description>Initial implementation</description>
    /// </item>
    /// <item>
    ///		<term>8/21/2020</term>
    ///		<term>Mauro Allende</term>
    ///		<term>RM-79</term>
    ///		<description>Refactor unit tests</description>  
    /// </item>
    /// </list>
    /// </remarks> 	
    [TestClass]
    public class AccountControllerTest
    {
        #region Private Constants

        private const string ExceptionMessage = "TestExceptionMessage";
        private const string PATHSearchBy = "api/v1/accounts";

        #endregion

        #region Private Members

        private readonly Mock<ICreditAccountRepository> _creditAccountRepositoryMock;
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<ICustomerDataProvider> _customerDataProviderMock;
        private readonly Mock<IAccountDataProvider> _accountDataProviderMock;
        private readonly Mock<ICreditAccountDataProvider> _creditAccountDataProviderMock;
        private readonly Mock<IAccountService> _accountService;
        private readonly CreditAccountStub _creditAccountStub;
        private readonly AccountStub _accountStub;
        private TestServer _server;
        private HttpClient _client;
        private HttpContent _postBody;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>      
        public AccountControllerTest()
        {
            _creditAccountRepositoryMock = new Mock<ICreditAccountRepository>();
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _customerDataProviderMock = new Mock<ICustomerDataProvider>();
            _creditAccountDataProviderMock = new Mock<ICreditAccountDataProvider>();
            _accountDataProviderMock = new Mock<IAccountDataProvider>();
            _creditAccountStub = new CreditAccountStub();
            _accountService = new Mock<IAccountService>();
            _accountStub = new AccountStub();

            CreateServer();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The test is Ok if Account for any customerId is found
        /// </summary>
        [TestMethod]
        public async Task GetAccountByCustomerDemographicsShouldReturnOkWithData()
        {
            // Arrange
            var accountSearchModel = new AccountSearchModel() { CreditAccountId = It.IsAny<decimal>() };

            _accountService.Setup(x => x.SearchBy(It.IsAny<AccountSearchModel>())).ReturnsAsync((string.Empty, new List<AccountModel>()));
            _postBody = new StringContent(JsonConvert.SerializeObject(accountSearchModel), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(PATHSearchBy, _postBody);
            var result = JsonConvert.DeserializeObject<List<AccountModel>>(response.Content.ReadAsStringAsync().Result);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsInstanceOfType(result, typeof(List<AccountModel>));
        }

        /// <summary>
        /// The test is Ok if we catch the exception and return an InternalServerError
        /// </summary>
        [TestMethod]
        public async Task GetAccountByCustomerDemographicsWithoutAddressCityStateZipParametersShouldReturnBadRequest()
        {
            // Act 
            var response = await _client.PostAsync(PATHSearchBy, _postBody);
            var result = response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task SearchByShouldReturnOK()
        {
            // Arrange
            var accountSearchModel = new AccountSearchModel() { CreditAccountId = It.IsAny<decimal>() };

            _postBody = new StringContent(JsonConvert.SerializeObject(accountSearchModel), Encoding.UTF8, "application/json");
            _accountService.Setup(x => x.SearchBy(It.IsAny<AccountSearchModel>())).ReturnsAsync((string.Empty, new List<AccountModel>()));

            // Act 
            var response = await _client.PostAsync(PATHSearchBy, _postBody);
            var result = JsonConvert.DeserializeObject<List<AccountModel>>(response.Content.ReadAsStringAsync().Result);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Create Test Server.
        /// </summary>      
        private void CreateServer()
        {
            _server = new TestServer(new WebHostBuilder()
               .UseStartup<StartupTest>()
               .ConfigureTestServices(services =>
               {
                   services.AddSingleton(_customerRepositoryMock.Object);
                   services.AddSingleton(_customerDataProviderMock.Object);
                   services.AddSingleton(_creditAccountRepositoryMock.Object);
                   services.AddSingleton(_creditAccountDataProviderMock.Object);
                   services.AddSingleton(_accountDataProviderMock.Object);
                   services.AddSingleton(_accountService.Object);
               })
               );

            _client = _server.CreateClient();
        }

        #endregion
    }
}
