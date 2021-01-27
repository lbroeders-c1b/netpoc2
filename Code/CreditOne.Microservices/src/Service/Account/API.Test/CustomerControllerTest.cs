using System.Collections.Generic;
using System.Linq;
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
    /// Implements the unit tests for customer controller class
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
    ///     <term>3/21/2019</term>
    ///     <term>Federico Bendayan</term>
    ///     <term>RM-47</term>
    ///     <description>Initial implementation</description>
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
    public class CustomerControllerTest
    {
        #region Private Constants

        private const string PATHGetCustomerBySSN = "api/v1/customers?ssn={0}";
        private const string PATHGetCustomerById = "api/v1/customers/{0}";
        private const string SsnString = "123456789";

        #endregion

        #region Private Members

        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<ICustomerDataProvider> _customerDataProviderMock;
        private readonly CustomerStub _customerStub;
        private readonly HttpContent _ssnPostBody;

        private TestServer _server;
        private HttpClient _client;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerControllerTest()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _customerDataProviderMock = new Mock<ICustomerDataProvider>();
            _customerStub = new CustomerStub();
            _ssnPostBody = new StringContent(JsonConvert.SerializeObject(SsnString), Encoding.UTF8, "application/json");

            CreateTestServer();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The test is Ok if Get returns a list of customers with their respectives ids
        /// </summary>
        [TestMethod]
        public async Task GetCustomerBySSNShouldReturnOkWithData()
        {
            // Arrange
            _customerDataProviderMock.Setup(x => x.GetBySSN(It.IsAny<string>()))
                .ReturnsAsync(new List<Customer> { _customerStub.GetValidCustomer() });

            // Act
            var response = await _client.PostAsync(string.Format(PATHGetCustomerBySSN, "1"), _ssnPostBody);
            var result = JsonConvert.DeserializeObject<CustomerModel>(response.Content.ReadAsStringAsync().Result);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CustomerModel));
        }

        /// <summary>
        /// The test is Ok if we return only one customer
        /// </summary>
        [TestMethod]
        public async Task GetCustomerByIdShouldReturnOnlyOneCustomer()
        {
            // Arrange
            _customerRepositoryMock.Setup(x => x.GetById(It.IsAny<decimal>()))
                .ReturnsAsync(_customerStub.GetAllCustomers().First());

            // Act
            var response = await _client.GetAsync(string.Format(PATHGetCustomerById, It.IsAny<decimal>()));
            var result = JsonConvert.DeserializeObject<CustomerModel>(response.Content.ReadAsStringAsync().Result);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsInstanceOfType(result, typeof(CustomerModel));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Create Test Server
        /// </summary>   
        private void CreateTestServer()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<StartupTest>()
                .ConfigureTestServices(services =>
                {
                    services.AddSingleton(_customerRepositoryMock.Object);
                    services.AddSingleton(_customerDataProviderMock.Object);
                })
                );

            _client = _server.CreateClient();
        }

        #endregion
    }
}
