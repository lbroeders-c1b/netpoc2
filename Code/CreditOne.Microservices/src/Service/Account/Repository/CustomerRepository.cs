using System.Linq;
using System.Threading.Tasks;

using CreditOne.Microservices.Account.Domain;
using CreditOne.Microservices.Account.IDataProvider;

namespace CreditOne.Microservices.Account.Repository
{
    /// <summary>
    /// Implements the customer repository class
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
    ///     <term>4/11/2019</term>
    ///     <term>Federico Bendayan</term>
    ///     <term>RM-47</term>
    ///     <description>Initial implementation</description>
    /// </item>
    /// <item>
    ///		<term>8/21/2020</term>
    ///		<term>Mauro Allende</term>
    ///		<term>RM-79</term>
    ///		<description>Cleanup code</description>  
    /// </item>
    /// </list>
    /// </remarks>
    public class CustomerRepository : ICustomerRepository
    {
        #region Private Members

        private readonly ICustomerDataProvider _customerDataProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>  
        public CustomerRepository(ICustomerDataProvider customerDataProvider)
        {
            _customerDataProvider = customerDataProvider;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the customer associated to the given ID
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>Customer</returns>
        public async Task<Customer> GetById(decimal customerId)
        {
            var customers = await _customerDataProvider.GetById(customerId);

            return customers.FirstOrDefault();
        }

        #endregion
    }
}
