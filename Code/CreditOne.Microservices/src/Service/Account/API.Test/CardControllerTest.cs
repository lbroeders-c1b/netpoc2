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
    /// Implements the unit tests for the card controller class
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///	    <term>Date</term>
    ///	    <term>Who</term>
    ///	    <term>BR/WO</term>
    ///	    <description>Description</description>
    /// </listheader>
    /// <item>
    ///	    <term>9/3/2019</term>
    ///	    <term>Federico Bendayan</term>
    ///	    <term>RM-47</term>
    ///	    <description>Initial implementation</description>
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
    public class CardControllerTest
    {
        #region Private Constants

        private const string PATHCards = "api/v1/creditaccounts/{0}/cards";

        #endregion

        #region Private Members

        private Mock<ICardDataProvider> _cardDataProviderMock;
        private TestServer _server;
        private HttpClient _client;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>      
        public CardControllerTest()
        {
            _cardDataProviderMock = new Mock<ICardDataProvider>();

            CreateServer();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The test is Ok if valid list of cards are found
        /// </summary>
        [TestMethod]
        public async Task GetByCreditAccountIdShouldReturnOkWithData()
        {
            ///Arrange
            var validCardListStub = new CardStub().GetValidCards();
            _cardDataProviderMock.Setup(mock => mock.GetByCreditAccountId(It.IsAny<decimal>())).ReturnsAsync(validCardListStub);

            ///Act
            var response = await _client.GetAsync(string.Format(PATHCards, It.IsAny<decimal>()));
            var result = JsonConvert.DeserializeObject<List<CardModel>>(response.Content.ReadAsStringAsync().Result);

            ///Assert
            _cardDataProviderMock.Verify(mock => mock.GetByCreditAccountId(It.IsAny<decimal>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsInstanceOfType(result, typeof(List<CardModel>));
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
               })
               );

            _client = _server.CreateClient();
        }

        #endregion
    }
}
