using System.Collections.Generic;
using System.Threading.Tasks;

using CreditOne.Microservices.Account.Domain;

namespace CreditOne.Microservices.Account.IDataProvider
{
    /// <summary>
    /// Defines the address data provider interface
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
    ///		<term>5/28/2019</term>
    ///		<term>Daniel Lobaton</term>
    ///		<term>RM-47</term>
    ///		<description>Initial implementation</description>
	/// </item>
    /// </list>
    /// </remarks> 	
    public interface IAddressDataProvider
    {
        /// <summary>
        /// Returns the associated Addresses to the credit account ID
        /// </summary>
        Task<IList<Address>> GetByCreditAccountId(decimal creditAccountId);
    }
}
