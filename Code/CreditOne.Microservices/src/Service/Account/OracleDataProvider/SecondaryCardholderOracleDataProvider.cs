using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using CreditOne.Microservices.Account.IDataProvider;
using CreditOne.Microservices.BuildingBlocks.Common.Configuration;
using CreditOne.Microservices.BuildingBlocks.OracleProvider.Core;

using Oracle.ManagedDataAccess.Client;

namespace CreditOne.Microservices.Account.OracleDataProvider
{
    /// <summary>
    /// Implements the secondary card holder oracle data provider class
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
	///		<term>10/3/2019</term>
	///		<term>Armando Soto</term>
	///		<term>RM-47</term>
	///		<description>Initial implementation</description>
	/// </item>
    /// </list>
    /// </remarks> 	
    public class SecondaryCardholderOracleDataProvider : OracleProviderBase, ISecondaryCardholderDataProvider
    {
        #region Private Constants

        private const string DBConnectionStringKey = "DBConnectionString";

        private const string PkgSecondaryCardholderProcedureGetByCreditAccountId = "PKG_MS_SECONDARY_CHD.PR_GET_BY_CREDIT_ACCT_ID";

        private const string ParameterCreditAccountId = "P_CREDIT_ACCOUNT_ID";

        private const string ColumnCreditAccountId = "CREDIT_ACCOUNT_ID";
        private const string ColumnSsnId = "SSN_ID";
        private const string ColumnSsn = "SSN";
        private const string ColumnDateOfBirth = "DATE_OF_BIRTH";
        private const string ColumnSecondaryCardholderdId = "SECONDARY_CHD_ID";
        private const string ColumnNameId = "NAME_ID";
        private const string ColumnName = "NAME";
        private const string ColumnVirtualFirstName = "VR_FIRST_NAME";
        private const string ColumnVirtualLastName = "VR_LAST_NAME";
        private const string CursorSecondaryCardholder = "CR_SECONDARY_CARDHOLDER";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>      
        public SecondaryCardholderOracleDataProvider(DatabaseFunctions databaseFunctions,
                                                     DbConnectionStrings dbConnectionStrings) :
            base(databaseFunctions, dbConnectionStrings)
        {
            ConnectionStringKey = DBConnectionStringKey;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the secondary card holder associated to the credit account ID
        /// </summary>
        /// <param name="creditAccountId">Credit account ID</param>
        /// <returns>List of secondary card holders</returns>
        public async Task<IList<Domain.SecondaryCardholder>> GetByCreditAccountId(decimal creditAccountId)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter(ParameterCreditAccountId, OracleDbType.Decimal, creditAccountId, ParameterDirection.Input)
            };

            return await GetAsync(PkgSecondaryCardholderProcedureGetByCreditAccountId, parameters,
                        new OracleParameter(CursorSecondaryCardholder, OracleDbType.RefCursor, ParameterDirection.Output), PopulateData);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Parses a database record for Secondary Cardholder data.
        /// </summary>
        /// <param name="record">Database record for containing Secondary Cardholder info</param>  
        /// <returns>The Secondary Cardholder Object with the SmartDataRecord values.</returns>
        private Domain.SecondaryCardholder PopulateData(SmartDataRecord record)
        {
            var secondaryCardholder = new Domain.SecondaryCardholder();

            if (!record.IsDBNull(ColumnCreditAccountId))
            {
                secondaryCardholder.CreditAccountId = record.GetDecimal(ColumnCreditAccountId);
            }

            if (!record.IsDBNull(ColumnSsnId))
            {
                secondaryCardholder.SsnId = record.GetDecimal(ColumnSsnId);
            }

            if (!record.IsDBNull(ColumnSsn))
            {
                secondaryCardholder.Ssn = record.GetString(ColumnSsn);
            }

            if (!record.IsDBNull(ColumnDateOfBirth))
            {
                secondaryCardholder.DateOfBirth = record.GetDateTime(ColumnDateOfBirth);
            }

            if (!record.IsDBNull(ColumnNameId))
            {
                secondaryCardholder.NameId = record.GetDecimal(ColumnNameId);
            }

            if (!record.IsDBNull(ColumnSecondaryCardholderdId))
            {
                secondaryCardholder.Id = record.GetDecimal(ColumnSecondaryCardholderdId);
            }

            if (!record.IsDBNull(ColumnVirtualFirstName))
            {
                secondaryCardholder.FirstName = record.GetString(ColumnVirtualFirstName);
            }

            if (!record.IsDBNull(ColumnVirtualLastName))
            {
                secondaryCardholder.LastName = record.GetString(ColumnVirtualLastName);
            }

            return secondaryCardholder;
        }

        #endregion
    }
}
