using System.Collections.Generic;
using System.Threading.Tasks;

namespace CreditOne.Microservices.Account.IDataProvider
{
    /// <summary>
    /// Defines the customer data provider interface
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
    ///     <term>5/30/2019</term>
    ///     <term>Federico Bendayan</term>
    ///     <term>RM-47</term>
    ///     <description>Initial implementation</description>
    /// </item>
    /// <item>
    ///		<term>8/21/2020</term>
    ///		<term>Mauro Allende</term>
    ///		<term>RM-79</term>
    ///		<description>Refactor</description>  
    /// </item>
    /// </list>
    /// </remarks>
    public interface ICustomerDataProvider
    {
        /// <summary>
        /// Retrieves the customers associated to the given social security number
        /// </summary>
        /// <param name="ssn">Social security number</param>
        /// <returns>List of customers</returns>
        Task<IList<Domain.Customer>> GetBySSN(string ssn);

        /// <summary>
        /// Retrieves the customer associated to the given customer ID
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>List of customers</returns>
        Task<IList<Domain.Customer>> GetById(decimal customerId);
    }
}
