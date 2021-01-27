using System.Collections.Generic;
using System.Threading.Tasks;

using CreditOne.Microservices.Account.Domain;

namespace CreditOne.Microservices.Account.IDataProvider
{
    /// <summary>
    /// Defines the card data provider interface
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
	/// </item>
    /// </list>
    /// </remarks> 	
    public interface ICardDataProvider
    {
        /// <summary>
        /// Retrieves the associated cards to the credit account ID
        /// </summary>
        Task<IList<Card>> GetByCreditAccountId(decimal creditAccountId);
    }
}
