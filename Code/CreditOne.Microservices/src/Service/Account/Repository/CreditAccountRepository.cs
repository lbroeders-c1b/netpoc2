using System.Collections.Generic;
using System.Threading.Tasks;

using CreditOne.Microservices.Account.Domain;
using CreditOne.Microservices.Account.IDataProvider;

namespace CreditOne.Microservices.Account.Repository
{
    /// <summary>
    /// Implements the credit account repository class
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
    ///		<description>Cleanup code</description>  
    /// </item>
    /// </list>
    /// </remarks
    public class CreditAccountRepository : ICreditAccountRepository
    {
        #region Private Members

        private readonly ICreditAccountDataProvider _creditAccountDataProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>   
        public CreditAccountRepository(ICreditAccountDataProvider creditAccountDataProvider)
        {
            _creditAccountDataProvider = creditAccountDataProvider;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the credit accounts by credit account ID
        /// </summary>
        /// <param name="creditAccountId">Credit account ID</param>
        /// <returns>List of credit accounts</returns>
        public async Task<IList<CreditAccount>> GetById(decimal creditAccountId)
        {
            return await _creditAccountDataProvider.GetById(creditAccountId);
        }

        #endregion
    }
}
