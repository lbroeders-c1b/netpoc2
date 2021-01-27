using System.Collections.Generic;
using System.Threading.Tasks;

namespace CreditOne.Microservices.Account.IDataProvider
{
    /// <summary>
    /// Defines the primary card holder data provider interface
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
    ///		<term>5/29/2019</term>
    ///		<term>Christian Azula</term>
    ///		<term>RM-47</term>
    ///		<description>Initial implementation</description>
    /// </item>
    /// <item>
    ///		<term>8/21/2020</term>
    ///		<term>Mauro Allende</term>
    ///		<term>RM-79</term>
    ///		<description>Cleanup code</description>  
    /// </item>
    /// </list>
    /// </remarks> 	
    public interface IPrimaryCardholderDataProvider
    {
        /// <summary>
        /// Retrieves the associated primary card holder to the credit account ID
        /// </summary>
        /// <param name="creditAccountId">Credit account ID</param>
        /// <returns>List of primary card holder</returns>
        Task<IList<Domain.PrimaryCardholder>> GetByCreditAccountId(decimal creditAccountId);
    }
}
