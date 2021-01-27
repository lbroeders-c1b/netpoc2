using System.Collections.Generic;
using System.Threading.Tasks;

using CreditOne.Microservices.Account.Domain;

namespace CreditOne.Microservices.Account.IDataProvider
{
    /// <summary>
    /// Defines the email data provider interface
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
    ///		<term>9/1/2020</term>
    ///		<term>Daniel Lobaton</term>
    ///		<term>RM-79</term>
    ///		<description>Initial implementation</description>
	/// </item>
    /// </list>
    /// </remarks> 	
    public interface IEmailDataProvider
    {
        /// <summary>
        /// Retrieves the associated emails to the credit account ID
        /// </summary>
        Task<IList<CreditAccountEmail>> GetByCreditAccountId(decimal creditAccountId);
    }
}
