using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using CreditOne.Microservices.Account.Domain.Test;
using CreditOne.Microservices.Account.IDataProvider;
using CreditOne.Microservices.Account.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace CreditOne.Microservices.Account.Service.Test
{
    /// <summary>
    /// Implements the unit test for Credit Account Service.
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
    ///		<term>11-20-2019</term>
    ///		<term>Armando Soto</term>
    ///		<term>RM-47</term>
    ///		<description>Initial implementation</description>  
    /// </item>
    /// </list>
    /// </remarks>
    [TestClass]
    public class AccountServiceTest
    {
        #region Private Members

        private readonly AccountService _accountService;

        private readonly Mock<ICreditAccountDataProvider> _creditAccountDataProvider;
        private readonly Mock<IAccountDataProvider> _accountDataProvider;
        private readonly Mock<ICustomerDataProvider> _customerDataProvider;
        private readonly Mock<IPrimaryCardholderDataProvider> _primaryCardholderDataProvider;
        private readonly Mock<ISecondaryCardholderDataProvider> _secondaryCardholderDataProvider;
        private readonly Mock<IAddressDataProvider> _addressDataProvider;
        private readonly Mock<IMapper> _mapper;

        private readonly CreditAccountStub _creditAccountResultStub;
        private readonly PrimaryCardholderStub _primaryCardholderStub;
        private readonly SecondaryCardholderStub _secondaryCardholderStub;
        private readonly AddressStub _addressStub;
        private readonly CustomerStub _customerStub;
        private readonly AccountStub _accountStub;

        private readonly string _inferedSsn = "Ssn";
        private readonly string _inferedCustomerId = "CustomerId";
        private readonly string _inferedCreditAccount = "CreditAccount";
        private readonly string _inferedCardNumber = "CardNumber";
        private readonly string _inferedDemographics = "Demographics";


        #endregion

        #region Constructor

        public AccountServiceTest()
        {
            _accountDataProvider = new Mock<IAccountDataProvider>();
            _creditAccountDataProvider = new Mock<ICreditAccountDataProvider>();
            _customerDataProvider = new Mock<ICustomerDataProvider>();
            _primaryCardholderDataProvider = new Mock<IPrimaryCardholderDataProvider>();
            _secondaryCardholderDataProvider = new Mock<ISecondaryCardholderDataProvider>();
            _addressDataProvider = new Mock<IAddressDataProvider>();
            _mapper = new Mock<IMapper>();

            _accountService = new AccountService(
                _accountDataProvider.Object,
                _customerDataProvider.Object,
                _creditAccountDataProvider.Object,
                _primaryCardholderDataProvider.Object,
                _secondaryCardholderDataProvider.Object,
                _addressDataProvider.Object,
                _mapper.Object);

            _primaryCardholderStub = new PrimaryCardholderStub();
            _secondaryCardholderStub = new SecondaryCardholderStub();
            _creditAccountResultStub = new CreditAccountStub();
            _addressStub = new AddressStub();
            _customerStub = new CustomerStub();
            _accountStub = new AccountStub();
        }

        #endregion

        [TestMethod]
        public async Task SearchByCardNumberShouldReturnTheInferedSearchAndAListOfAccountModel()
        {
            ///Arrange 
            var cardValidCreditAccount = _creditAccountResultStub.GetAll().First();
            _creditAccountDataProvider.Setup(x => x.GetByCardNumber(It.IsAny<string>())).ReturnsAsync(_creditAccountResultStub.GetAll());
            _primaryCardholderDataProvider.Setup(x => x.GetByCreditAccountId(It.IsAny<decimal>())).ReturnsAsync(_primaryCardholderStub.GetPrimaryCardholders());
            _addressDataProvider.Setup(x => x.GetByCreditAccountId(It.IsAny<decimal>())).ReturnsAsync(_addressStub.GetAll());
            _secondaryCardholderDataProvider.Setup(x => x.GetByCreditAccountId(It.IsAny<decimal>())).ReturnsAsync(_secondaryCardholderStub.GetSecondaryCardholders());
            _customerDataProvider.Setup(x => x.GetById(It.IsAny<decimal>())).ReturnsAsync(_customerStub.GetAllCustomers());

            ///Act
            var result = await _accountService.SearchBy(new AccountSearchModel() { CardNumber = "1234567891234567" });

            ///Assert
            Assert.AreEqual(_inferedCardNumber, result.inferedSearch);
            Assert.IsTrue(result.accountModels.Count() > 0);
        }

        [TestMethod]
        public async Task SearchBySsnShouldReturnTheInferedSearchAndAListOfAccountModel()
        {
            ///Arrange 
            _creditAccountDataProvider.Setup(x => x.GetByCardNumber(It.IsAny<string>())).ReturnsAsync(_creditAccountResultStub.GetAll());
            _primaryCardholderDataProvider.Setup(x => x.GetByCreditAccountId(It.IsAny<decimal>())).ReturnsAsync(_primaryCardholderStub.GetPrimaryCardholders());
            _addressDataProvider.Setup(x => x.GetByCreditAccountId(It.IsAny<decimal>())).ReturnsAsync(_addressStub.GetAll());
            _secondaryCardholderDataProvider.Setup(x => x.GetByCreditAccountId(It.IsAny<decimal>())).ReturnsAsync(_secondaryCardholderStub.GetSecondaryCardholders());
            _creditAccountDataProvider.Setup(x => x.GetByCustomerId(It.IsAny<decimal>())).ReturnsAsync(_creditAccountResultStub.GetAll());
            _customerDataProvider.Setup(x => x.GetBySSN(It.IsAny<string>())).ReturnsAsync(_customerStub.GetAllCustomers());

            ///Act
            var result = await _accountService.SearchBy(new AccountSearchModel() { Ssn = "123456789" });

            ///Assert
            Assert.AreEqual(_inferedSsn, result.inferedSearch);
            Assert.IsTrue(result.accountModels.Count() > 0);
        }

        [TestMethod]
        public async Task SearchByCreditAccountIdShouldReturnTheInferedSearchAndAListOfAccountModel()
        {
            ///Arrange 
            _creditAccountDataProvider.Setup(x => x.GetByCardNumber(It.IsAny<string>())).ReturnsAsync(_creditAccountResultStub.GetAll());
            _primaryCardholderDataProvider.Setup(x => x.GetByCreditAccountId(It.IsAny<decimal>())).ReturnsAsync(_primaryCardholderStub.GetPrimaryCardholders());
            _addressDataProvider.Setup(x => x.GetByCreditAccountId(It.IsAny<decimal>())).ReturnsAsync(_addressStub.GetAll());
            _secondaryCardholderDataProvider.Setup(x => x.GetByCreditAccountId(It.IsAny<decimal>())).ReturnsAsync(_secondaryCardholderStub.GetSecondaryCardholders());
            _creditAccountDataProvider.Setup(x => x.GetById(It.IsAny<decimal>())).ReturnsAsync(_creditAccountResultStub.GetAll());
            _customerDataProvider.Setup(x => x.GetById(It.IsAny<decimal>())).ReturnsAsync(_customerStub.GetAllCustomers());

            ///Act
            var result = await _accountService.SearchBy(new AccountSearchModel() { CreditAccountId = 1234 });

            ///Assert
            Assert.AreEqual(_inferedCreditAccount, result.inferedSearch);
            Assert.IsTrue(result.accountModels.Count() > 0);
        }

        [TestMethod]
        public async Task SearchByCustomerIdShouldReturnTheInferedSearchAndAListOfAccountModel()
        {
            ///Arrange 
            _creditAccountDataProvider.Setup(x => x.GetByCardNumber(It.IsAny<string>())).ReturnsAsync(_creditAccountResultStub.GetAll());
            _primaryCardholderDataProvider.Setup(x => x.GetByCreditAccountId(It.IsAny<decimal>())).ReturnsAsync(_primaryCardholderStub.GetPrimaryCardholders());
            _addressDataProvider.Setup(x => x.GetByCreditAccountId(It.IsAny<decimal>())).ReturnsAsync(_addressStub.GetAll());
            _secondaryCardholderDataProvider.Setup(x => x.GetByCreditAccountId(It.IsAny<decimal>())).ReturnsAsync(_secondaryCardholderStub.GetSecondaryCardholders());
            _creditAccountDataProvider.Setup(x => x.GetByCustomerId(It.IsAny<decimal>())).ReturnsAsync(_creditAccountResultStub.GetAll());
            _customerDataProvider.Setup(x => x.GetById(It.IsAny<decimal>())).ReturnsAsync(_customerStub.GetAllCustomers());

            ///Act
            var result = await _accountService.SearchBy(new AccountSearchModel() { CustomerId = 1234 });

            ///Assert
            Assert.AreEqual(_inferedCustomerId, result.inferedSearch);
            Assert.IsTrue(result.accountModels.Count() > 0);
        }

        [TestMethod]
        public async Task SearchByDemographicsShouldReturnTheInferedSearchAndAListOfAccountModel()
        {
            ///Arrange 
            _accountDataProvider.Setup(x => x.GetByCustomerDemographics(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<Domain.Account>());
            _accountDataProvider.Setup(x => x.GetByCustomerDemographics(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(_accountStub.GetAllAccounts());

            ///Act
            var result = await _accountService.SearchBy(new AccountSearchModel() { AddressLine1 = "Address", City = "City", State = "State", Zip = "Zip" });

            ///Assert
            Assert.AreEqual(_inferedDemographics, result.inferedSearch);
        }
    }
}