using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using CreditOne.Microservices.Account.Domain;
using CreditOne.Microservices.Account.IDataProvider;
using CreditOne.Microservices.Account.Models;
using CreditOne.Microservices.BuildingBlocks.Types;

namespace CreditOne.Microservices.Account.Service
{
    /// <summary>
    /// Provides logic for a customer account which orchastrates its child entities.
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
    ///		<term>10/01/2019</term>
    ///		<term>Luis Petitjean</term>
    ///		<term>RM-47</term>
    ///		<description>Initial Implementation</description>
    /// </item>    
    /// </list>
    /// </remarks>
    public class AccountService : IAccountService
    {
        #region  Private Members 

        private readonly IAccountDataProvider _accountDataProvider;
        private readonly ICustomerDataProvider _customerDataProvider;
        private readonly ICreditAccountDataProvider _creditAccountDataProvider;
        private readonly IPrimaryCardholderDataProvider _primaryCardholderDataProvider;
        private readonly ISecondaryCardholderDataProvider _secondaryCardholderDataProvider;
        private readonly IAddressDataProvider _addressDataProvider;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs the <c>AccountService</c>
        /// </summary>
        /// <param name="accountDataProvider"></param>
        /// <param name="customerDataProvider"></param>
        /// <param name="creditAccountDataProvider"></param>
        /// <param name="primaryCardholderDataProvider"></param>
        /// <param name="secondaryCardholderDataProvider"></param>
        /// <param name="addressDataProvider"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public AccountService(IAccountDataProvider accountDataProvider, ICustomerDataProvider customerDataProvider,
                              ICreditAccountDataProvider creditAccountDataProvider, IPrimaryCardholderDataProvider primaryCardholderDataProvider,
                              ISecondaryCardholderDataProvider secondaryCardholderDataProvider, IAddressDataProvider addressDataProvider,
                              IMapper mapper)
        {
            _accountDataProvider = accountDataProvider;
            _customerDataProvider = customerDataProvider;
            _creditAccountDataProvider = creditAccountDataProvider;
            _primaryCardholderDataProvider = primaryCardholderDataProvider;
            _secondaryCardholderDataProvider = secondaryCardholderDataProvider;
            _addressDataProvider = addressDataProvider;
            _mapper = mapper;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Searches the customer account data by a given criteria.
        /// </summary>
        /// <param name="accountSearchModel">The search criteria</param>
        /// <returns>The <c>inferedSearch</c> and a list of the <c>AccountModel</c>.</returns>
        public async Task<(string inferedSearch, List<AccountModel> accountModels)> SearchBy(AccountSearchModel accountSearchModel)
        {
            switch (GetCriteria(accountSearchModel))
            {
                case SearchAccountBy.CardNumber:
                    {
                        return (SearchAccountBy.CardNumber.ToString(), await SearchByCardNumber(accountSearchModel.CardNumber));
                    }

                case SearchAccountBy.Ssn:
                    {
                        return (SearchAccountBy.Ssn.ToString(), await SearchBySsn(accountSearchModel.Ssn));
                    }

                case SearchAccountBy.CustomerId:
                    {
                        return (SearchAccountBy.CustomerId.ToString(), await SearchByCustomerId(accountSearchModel.CustomerId.Value));
                    }

                case SearchAccountBy.CreditAccount:
                    {
                        return (SearchAccountBy.CreditAccount.ToString(), await SearchByCreditAccountId(accountSearchModel.CreditAccountId.Value));
                    }

                case SearchAccountBy.Demographics:
                    {
                        return (SearchAccountBy.Demographics.ToString(), await SearchByDemographics(accountSearchModel));
                    }
            }
            return (SearchAccountBy.Undefined.ToString(), null);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the search type criteria by the given request model.
        /// </summary>
        /// <param name="accountSearchModel">The search criteria</param>
        /// <returns>The search type infered from <c>AccountSearchModel</c>.</returns>
        private SearchAccountBy GetCriteria(AccountSearchModel accountSearchModel)
        {
            var searchAccountBy = SearchAccountBy.Undefined;

            if (accountSearchModel.CardNumber.IsCardLengthInRange())
            {
                searchAccountBy = SearchAccountBy.CardNumber;
            }
            else if (SocialSecurityNumber.IsValidSocialSecurityNumber(accountSearchModel.Ssn))
            {
                searchAccountBy = SearchAccountBy.Ssn;
            }
            else if (accountSearchModel.CreditAccountId.HasValue && accountSearchModel.CreditAccountId.Value >= 1)
            {
                searchAccountBy = SearchAccountBy.CreditAccount;
            }
            else if (accountSearchModel.CustomerId.HasValue && accountSearchModel.CustomerId.Value >= 1)
            {
                searchAccountBy = SearchAccountBy.CustomerId;
            }
            else
            {
                searchAccountBy = SearchAccountBy.Demographics;
            }

            return searchAccountBy;
        }

        /// <summary>
        /// Searches the customer account by card number.
        /// </summary>
        /// <param name="cardNumber">The card number</param>
        /// <returns>A list of one <c>AccountModel</c> item  or <c>null</c> when no match is found.</returns>
        private async Task<List<AccountModel>> SearchByCardNumber(string cardNumber)
        {
            var creditAccounts = (await _creditAccountDataProvider.GetByCardNumber(cardNumber)) as List<CreditAccount>;

            if (creditAccounts?.Count > 0)
            {
                var customer = (await _customerDataProvider.GetById(creditAccounts.FirstOrDefault().CustomerId)).FirstOrDefault();

                return await MapAccountModel(customer, creditAccounts);
            }

            return null;
        }

        /// <summary>
        /// Searches the customer account by the given SSN
        /// </summary>
        /// <param name="ssn">The Social Security Number</param>
        /// <returns>A list of one or more <c>AccountModel</c> items or <c>null</c> when no match is found.</returns>
        private async Task<List<AccountModel>> SearchBySsn(string ssn)
        {
            var customer = (await _customerDataProvider.GetBySSN(ssn)).FirstOrDefault();

            if (customer != null)
            {
                var creditAccounts = (await _creditAccountDataProvider.GetByCustomerId(customer.Id)).ToList();

                return await MapAccountModel(customer, creditAccounts);
            }

            return null;
        }

        /// <summary>
        /// Maps the <c>List<AccountModel></c> by the given domain objects
        /// </summary>
        /// <param name="ssn">The social security number</param>
        /// <param name="customer">The customer domain object</param>
        /// <param name="creditAccounts">A list of credit accounts</param>
        /// <returns>A list of one or more <c>AccountModel</c> items or <c>null</c> when no match is found.</returns>
        private async Task<List<AccountModel>> MapAccountModel(Customer customer, List<CreditAccount> creditAccounts)
        {
            List<AccountModel> accountModels = null;

            if (customer != null && creditAccounts?.Count > 0)
            {
                accountModels = new List<AccountModel>();

                foreach (CreditAccount creditAccount in creditAccounts)
                {
                    AccountModel accountModel = await MapAccountModelFromCreditAccountAggregates(creditAccount);

                    if (accountModel != null)
                    {
                        accountModel.CustomerId = customer.Id;

                        accountModel.Last4SSN = customer.SocialSecurityNumber.LastFour;

                        accountModels.Add(accountModel);
                    }

                }

                return accountModels;
            }

            return null;
        }

        /// <summary>
        /// Searches the customer account by the given credit account
        /// </summary>
        /// <param name="creditAccountId">The credit account ID</param>
        /// <returns>A list of one <c>AccountModel</c> item or <c>null</c> when no match is found.</returns>
        private async Task<List<AccountModel>> SearchByCreditAccountId(decimal creditAccountId)
        {
            var creditAccounts = (await _creditAccountDataProvider.GetById(creditAccountId)) as List<CreditAccount>;

            if (creditAccounts?.Count > 0)
            {
                var customer = (await _customerDataProvider.GetById(creditAccounts.FirstOrDefault().CustomerId)).FirstOrDefault();

                return await MapAccountModel(customer, creditAccounts);
            }

            return null;
        }

        /// <summary>
        /// Searches the customer account by the given customer ID
        /// </summary>
        /// <param name="customerId">The customer ID</param>
        /// <returns>A list of one or more <c>AccountModel</c> items or <c>null</c> when no match is found.</returns>
        private async Task<List<AccountModel>> SearchByCustomerId(decimal customerId)
        {
            var customer = (await _customerDataProvider.GetById(customerId)).FirstOrDefault();

            if (customer != null)
            {
                var creditAccounts = (await _creditAccountDataProvider.GetByCustomerId(customer.Id)).ToList();

                return await MapAccountModel(customer, creditAccounts);
            }

            return null;
        }

        /// <summary>
        /// Searches the customer account by a group of properties set by the given criteria
        /// </summary>
        /// <param name="accountSearchModel">Conatins the properties set for the search</param>
        /// <returns>A list of one or more <c>AccountModel</c> items or <c>null</c> when no match is found.</returns>
        private async Task<List<AccountModel>> SearchByDemographics(AccountSearchModel accountSearchModel)
        {
            List<AccountModel> accountModels = null;

            List<Domain.Account> accounts = (await _accountDataProvider.
                    GetByCustomerDemographics(
                                        accountSearchModel.HomePhone,
                                        accountSearchModel.WorkPhone,
                                        accountSearchModel.LastName,
                                        accountSearchModel.FirstName,
                                        accountSearchModel.MiddleInitial,
                                        accountSearchModel.CardPrefix,
                                        accountSearchModel.CardType,
                                        accountSearchModel.State,
                                        accountSearchModel.Zip,
                                        accountSearchModel.Exactness)).ToList();

            if (!accounts.Any() &&
                !string.IsNullOrEmpty(accountSearchModel.AddressLine1) &&
                !string.IsNullOrEmpty(accountSearchModel.City) &&
                !string.IsNullOrEmpty(accountSearchModel.State) &&
                !string.IsNullOrEmpty(accountSearchModel.Zip))
            {
                accounts = (await _accountDataProvider.GetByCustomerDemographics(
                                       accountSearchModel.AddressLine1,
                                       accountSearchModel.AddressLine2,
                                       accountSearchModel.City,
                                       accountSearchModel.State,
                                       accountSearchModel.Zip)).ToList();
            }
            if (accounts.Any())
            {
                accountModels = _mapper.Map<List<AccountModel>>(accounts);
            }

            return accountModels;
        }

        /// <summary>
        /// Gets the credit account aggregates and maps them along with the credit account to the <c>AccountModel</c> 
        /// </summary>
        /// <param name="creditAccount">The credit account domain object</param>
        /// <returns>An <c>AccountModel</c>or <c>null</c> when a mandatory credit accounnt child object is not found</returns>
        private async Task<AccountModel> MapAccountModelFromCreditAccountAggregates(CreditAccount creditAccount)
        {
            var primaryCardholder = (await _primaryCardholderDataProvider.GetByCreditAccountId(creditAccount.Id)).FirstOrDefault();

            if (primaryCardholder != null)
            {
                var address = (await _addressDataProvider.GetByCreditAccountId(creditAccount.Id)).FirstOrDefault();

                if (address != null)
                {
                    var secondaryCardholder = (await _secondaryCardholderDataProvider.GetByCreditAccountId(creditAccount.Id)).FirstOrDefault();

                    return new AccountModel()
                    {
                        CreditAccountId = creditAccount.Id,
                        PrimaryName = primaryCardholder.PersonName.FdrName,
                        SecondaryName = secondaryCardholder?.PersonName.FdrName,
                        AddressLine1 = address.AddressLineOne,
                        AddressLine2 = address.AddressLineTwo,
                        CardNumber = creditAccount.Card.BankCardNumber.Full,
                        City = address.City,
                        State = address.State,
                        Zip = address.Zip,
                        ExternalStatus = creditAccount.Card.ExternalStatusCode,
                        InternalStatusCode = creditAccount.Card.InternalStatusCode,
                        SystemOfRecord = creditAccount.Card.SystemOfRecord
                    };
                }
            }

            return null;
        }
        /// <summary>
        /// Identifies the search types
        /// </summary>
        private enum SearchAccountBy
        {
            Undefined = 0,
            Ssn = 1,
            CustomerId = 2,
            CreditAccount = 3,
            CardNumber = 4,
            Demographics = 5
        }

        #endregion
    }
}
