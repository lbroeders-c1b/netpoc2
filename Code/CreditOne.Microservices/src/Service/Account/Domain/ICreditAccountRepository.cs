using System.Collections.Generic;
using System.Threading.Tasks;

namespace CreditOne.Microservices.Account.Domain
{
    /// <summary>
    /// Defines the credit account repository interface
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
    /// <item>
    ///		<term>8/21/2020</term>
    ///		<term>Mauro Allende</term>
    ///		<term>RM-79</term>
    ///		<description>Refactor</description>  
    /// </item>
    /// </list>
    /// </remarks> 
    public interface ICreditAccountRepository
    {
        /// <summary>
        /// Returns the list of credit sccounts by credit account ID
        /// </summary>
        /// <param name="creditAccountId">Credit account ID</param>
        /// <returns>List of credit accounts</returns>
        Task<IList<CreditAccount>> GetById(decimal creditAccountId);
    }
}
