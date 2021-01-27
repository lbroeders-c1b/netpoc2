using System.Collections.Generic;
using System.Threading.Tasks;

namespace CreditOne.Microservices.Account.IDataProvider
{
    /// <summary>
    /// Defines the credit accoun data provider interface
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
    ///		<term>5/27/2019</term>
    ///		<term>Armando Soto</term>
    ///		<term>RM-47</term>
    ///		<description>Initial implementation</description>
    ///	</item>
    /// </list>
    /// </remarks> 
    public interface ICreditAccountDataProvider
    {
        /// <summary>
        /// Retrieves the credit accounts for the given credit account ID
        /// </summary>
        /// <param name="creditAccountId">Credit account ID</param>
        /// <returns>List of credit accounts</returns>
        Task<IList<Domain.CreditAccount>> GetById(decimal creditAccountId);

        /// <summary>
        /// Retrieves the credit accounts for the given customer ID
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>List of credit account</returns>
        Task<IList<Domain.CreditAccount>> GetByCustomerId(decimal customerId);

        /// <summary>
        /// Retrieves the credit accounts for the given card number
        /// </summary>
        /// <param name="cardNumber">Card number</param>
        /// <returns>List of credit accounts</returns>
        Task<IList<Domain.CreditAccount>> GetByCardNumber(string cardNumber);
    }
}
