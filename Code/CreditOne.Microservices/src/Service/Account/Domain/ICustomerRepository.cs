using System.Threading.Tasks;

namespace CreditOne.Microservices.Account.Domain
{
    /// <summary>
    /// Defines the customer repository interface
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
    public interface ICustomerRepository
    {
        /// <summary>
        /// Returns the customer associated to the given ID
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>Customer information</returns>
        Task<Domain.Customer> GetById(decimal customerId);
    }
}
