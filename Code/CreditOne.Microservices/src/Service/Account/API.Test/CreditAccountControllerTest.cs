using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using CreditOne.Microservices.Account.Domain;
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
    /// Implements the unit tests for the credit account controller class
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
    ///		<term>5/27/2019</term>
    ///		<term>Armando Soto</term>
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
    public class CreditAccountControllerTest
    {
        #region Private Constants

        private const string CardNumberString = "1234567891234567";
        private const string PATHGetByCustomerId = "api/v1/creditaccounts?customerId={0}";
        private const string PATHGetById = "api/v1/creditaccounts/{0}";
        private const string PATHGetCreditAccountByCardNumber = "api/v1/creditaccounts";
        private const string ErrorMessageBadNumberAccountFound = "More than one account found";

        #endregion

        #region Private Members

        private readonly Mock<ICreditAccountRepository> _creditAccountRepositoryMock;
        private readonly Mock<ICreditAccountDataProvider> _creditAccountDataProviderMock;
        private readonly Mock<ICardDataProvider> _cardDataProviderMock;
        private readonly CreditAccountStub _creditAccountStub;
        private readonly HttpContent _cardPostBody;

        private TestServer _server;
        private HttpClient _client;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>      
        public CreditAccountControllerTest()
        {
            _creditAccountRepositoryMock = new Mock<ICreditAccountRepository>();
            _creditAccountDataProviderMock = new Mock<ICreditAccountDataProvider>();
            _cardDataProviderMock = new Mock<ICardDataProvider>();
            _creditAccountStub = new CreditAccountStub();
            _cardPostBody = new StringContent(JsonConvert.SerializeObject(CardNumberString), Encoding.UTF8, "application/json");

            CreateServer();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The test is Ok if we return only one credit account with it's curren active card
        /// </summary>
        [TestMethod]
        public async Task GetCreditAccountByIdShouldReturnOnlyOneCreditAccountWithItsCurrentActiveCard()
        {
            // Arrange
            _creditAccountRepositoryMock.Setup(x => x.GetById(It.IsAny<decimal>()))
                .ReturnsAsync(_creditAccountStub.GetCreditAccountWithAnActiveCard());

            // Act
            var response = await _client.GetAsync(string.Format(PATHGetById, It.IsAny<decimal>()));
            var result = JsonConvert.DeserializeObject<CreditAccountModel>(response.Content.ReadAsStringAsync().Result);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsInstanceOfType(result, typeof(CreditAccountModel));
            Assert.IsNotNull(result.Card);
            Assert.IsTrue(result.Card.IsCurrent);
        }

        /// <summary>
        /// The test is Ok if CreditAccount for any customerId is found
        /// </summary>
        [TestMethod]
        public async Task GetByCustomerIdShouldReturnOkWithData()
        {
            // Arrange
            _creditAccountDataProviderMock.Setup(x => x.GetByCustomerId(It.IsAny<decimal>()))
                .ReturnsAsync(_creditAccountStub.GetAll());

            // Act
            var response = await _client.GetAsync(string.Format(PATHGetByCustomerId, It.IsAny<decimal>()));
            var result = JsonConvert.DeserializeObject<List<CreditAccountModel>>(response.Content.ReadAsStringAsync().Result);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsInstanceOfType(result, typeof(List<CreditAccountModel>));
        }

        /// <summary>
        /// This test check if all the CreditAccount find for that custommer brings just the current active card
        /// </summary>
        [TestMethod]
        public async Task GetByCustomerIdShouldReturnAllCreditAccountsWithTheCurrentCard()
        {
            // Arrange
            _creditAccountDataProviderMock.Setup(x => x.GetByCustomerId(It.IsAny<decimal>()))
                .ReturnsAsync(_creditAccountStub.GetCreditAccountWithAnActiveCard());

            // Act
            var response = await _client.GetAsync(string.Format(PATHGetByCustomerId, It.IsAny<decimal>()));
            var result = JsonConvert.DeserializeObject<List<CreditAccountModel>>(response.Content.ReadAsStringAsync().Result);

            // Assert
            Assert.IsNull(result.Find(x => x.Card == null));
            Assert.IsNull(result.Find(x => !x.Card.IsCurrent));
        }

        /// <summary>
        /// The test is Ok if the Method return a credit account and OK as response 
        /// </summary>
        [TestMethod]
        public async Task GetCreditAccountByCardNumberShouldReturnOk()
        {
            // Arrange           
            _creditAccountDataProviderMock.Setup(x => x.GetByCardNumber(It.IsAny<string>()))
                .ReturnsAsync(_creditAccountStub.GetCreditAccountWithAnActiveCard());

            // Act 
            var response = await _client.PostAsync(PATHGetCreditAccountByCardNumber, _cardPostBody);
            var result = JsonConvert.DeserializeObject<CreditAccountModel>(response.Content.ReadAsStringAsync().Result);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsInstanceOfType(result, typeof(CreditAccountModel));
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
                   services.AddSingleton(_cardDataProviderMock.Object);
                   services.AddSingleton(_creditAccountRepositoryMock.Object);
                   services.AddSingleton(_creditAccountDataProviderMock.Object);
               })
               );

            _client = _server.CreateClient();
        }

        #endregion
    }
}