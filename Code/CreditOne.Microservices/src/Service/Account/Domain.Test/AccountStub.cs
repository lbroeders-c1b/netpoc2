using System.Collections.Generic;
using System.Linq;

namespace CreditOne.Microservices.Account.Domain.Test
{
    /// <summary>
    /// Implements the test data for unit testing the api
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
    /// </list>
    /// </remarks> 
    public class AccountStub
    {
        #region Private Members

        private readonly List<Account> _accounts;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public AccountStub()
        {
            _accounts = new List<Account>()
            {
                  new Account
                  {
                      CardNumber = "456012547896",
                      PrimaryName = "John",
                      SecondaryName = "Arnold",
                      AddressLine1 = "Pilot Road",
                      City = "Las Vegas"
                  }
            };
        }

        #endregion

        #region Public Memebers

        /// <summary>
        /// Mocks a customer.
        /// </summary>
        /// <returns> Valid customer</returns>
        public Account GetValidAccount()
        {
            return _accounts.First();
        }

        /// <summary>
        /// Mocks a empty customer.
        /// </summary>
        /// <returns> Empty customer</returns>
        public List<Domain.Account> GetEmptyAccounts()
        {
            return new List<Domain.Account>();
        }

        /// <summary>
        /// Returns the list of all customers
        /// </summary>
        /// <returns>List<Domain.Customer></returns>
        public List<Account> GetAllAccounts()
        {
            return _accounts;
        }

        #endregion
    }
}
