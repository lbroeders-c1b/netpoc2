using CreditOne.Microservices.Account.Domain.Test;
using CreditOne.Microservices.Account.IDataProvider;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace CreditOne.Microservices.Account.Repository.Test
{
    /// <summary>
    /// Implements the unit test for Customer Repository.
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
    ///		<term>11-13-2019</term>
    ///		<term>Christian Azula</term>
    ///		<term>RM-47</term>
    ///		<description>Initial implementation</description>  
    /// </item>
    /// </list>
    /// </remarks>
    [TestClass]
    public class CustomerRepositoryTest
    {
        #region Private Members

        private readonly Mock<ICustomerDataProvider> _customerDataProviderMock;
        private readonly CustomerRepository _customerRepository;
        private readonly CustomerStub _customerResultStub;

        #endregion

        #region Constructor

        public CustomerRepositoryTest()
        {
            _customerDataProviderMock = new Mock<ICustomerDataProvider>();
            _customerRepository = new CustomerRepository(_customerDataProviderMock.Object);
            _customerResultStub = new CustomerStub();
        }

        #endregion

        [TestMethod]
        public void GetByIdShouldReturnAnCustomer()
        {
            // Arrange
            _customerDataProviderMock.Setup(x => x.GetById(It.IsAny<decimal>())).ReturnsAsync(_customerResultStub.GetAllCustomers());

            // Act
            var result = _customerRepository.GetById(It.IsAny<decimal>()).Result;

            // Asert
            Assert.IsNotNull(result);
        }
    }
}
