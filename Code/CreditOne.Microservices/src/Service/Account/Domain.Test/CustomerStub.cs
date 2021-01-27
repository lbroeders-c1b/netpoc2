using System.Collections.Generic;
using System.Linq;

namespace CreditOne.Microservices.Account.Domain.Test
{
    /// <summary>
    /// Implements test data for unit testing the api
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
    public class CustomerStub
    {
        #region Private Members

        private readonly IList<Customer> _customers;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerStub()
        {
            _customers = new List<Customer>()
            {
                  new Customer
                  {
                      Id = 1,
                      Ssn = "123456789",
                      ExperianPin = "123"
                  }
            };
        }

        #endregion

        #region Public Memebers

        /// <summary>
        /// Mocks a customer
        /// </summary>
        /// <returns> Valid customer</returns>
        public Customer GetValidCustomer()
        {
            return _customers.First();
        }

        /// <summary>
        /// Mocks a empty customer.
        /// </summary>
        /// <returns> Empty customer</returns>
        public Customer GetEmptyCustomer()
        {
            return null;
        }

        /// <summary>
        /// Return the list of all customers
        /// </summary>
        /// <returns>List<Domain.Customer></returns>
        public IList<Customer> GetAllCustomers()
        {
            return _customers;
        }

        #endregion
    }
}
