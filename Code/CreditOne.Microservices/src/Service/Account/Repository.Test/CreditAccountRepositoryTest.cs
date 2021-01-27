using CreditOne.Microservices.Account.Domain.Test;
using CreditOne.Microservices.Account.IDataProvider;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace CreditOne.Microservices.Account.Repository.Test
{
    /// <summary>
    /// Implements the unit test for Credit Account Repository.
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
    public class CreditAccountRepositoryTest
    {
        #region Private Members

        private readonly Mock<ICreditAccountDataProvider> _creditAccountDataProviderMock;
        private readonly CreditAccountRepository _creditAccountRepository;
        private readonly CreditAccountStub _creditAccountResultStub;

        #endregion

        #region Constructor

        public CreditAccountRepositoryTest()
        {
            _creditAccountDataProviderMock = new Mock<ICreditAccountDataProvider>();
            _creditAccountRepository = new CreditAccountRepository(_creditAccountDataProviderMock.Object);
            _creditAccountResultStub = new CreditAccountStub();
        }

        #endregion

        [TestMethod]
        public void GetByIdShouldReturnAllCreditAccountsWithTheCurrentCard()
        {
            // Arrange
            _creditAccountDataProviderMock.Setup(x => x.GetById(It.IsAny<decimal>())).ReturnsAsync(_creditAccountResultStub.GetCreditAccountWithAnActiveCard());

            // Act
            var result = _creditAccountRepository.GetById(It.IsAny<decimal>()).Result;

            // Asert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
        }
    }
}
