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
    /// Implements the primary card holder oracle data provider class
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
    ///	</item>
    /// </list>
    /// </remarks>
    public class PrimaryCardholderOracleDataProvider : OracleProviderBase, IPrimaryCardholderDataProvider
    {
        #region Private Constants

        private const string DBConnectionStringKey = "DBConnectionString";

        private const string PkgPrimaryCardholderProcedureGetByCreditAccountId = "PKG_MS_PRIMARY_CHD.PR_GET_BY_CREDIT_ACCT_ID";

        private const string ParameterCreditAccountId = "P_CREDIT_ACCOUNT_ID";

        private const string ColumnCreditAccountId = "CREDIT_ACCOUNT_ID";
        private const string ColumnCreditBureauReportingFlag = "CREDIT_BUREAU_RPTG_FLAG";
        private const string ColumnCustomerId = "CUSTOMER_ID";
        private const string ColumnDateOfBirth = "DATE_OF_BIRTH";
        private const string ColumnPrimaryCardholderdId = "PRIMARY_CHD_ID";
        private const string ColumnNameId = "NAME_ID";
        private const string ColumnName = "NAME";
        private const string ColumnVirtualFirstName = "VR_FIRST_NAME";
        private const string ColumnVirtualLastName = "VR_LAST_NAME";

        private const string CursorPrimaryCardholder = "CR_PRIMARY_CARDHOLDER";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>   
        public PrimaryCardholderOracleDataProvider(DatabaseFunctions databaseFunctions,
                                                   DbConnectionStrings dbConnectionStrings) :
            base(databaseFunctions, dbConnectionStrings)
        {
            ConnectionStringKey = DBConnectionStringKey;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns Primary Cardholder associated to the credit account ID
        /// </summary>
        /// <param name="creditAccountId">Credit account ID</param>
        /// <returns>List of primary card holders</returns>
        public async Task<IList<Domain.PrimaryCardholder>> GetByCreditAccountId(decimal creditAccountId)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter(ParameterCreditAccountId, OracleDbType.Decimal, creditAccountId, ParameterDirection.Input)
            };

            return await GetAsync(PkgPrimaryCardholderProcedureGetByCreditAccountId, parameters,
                        new OracleParameter(CursorPrimaryCardholder, OracleDbType.RefCursor, ParameterDirection.Output), PopulatePrimaryCardholderData);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Parses a database record for Primary Cardholder data.
        /// </summary>
        /// <param name="record">Database record for containing Primary Cardholder info</param>  
        /// <returns>The Primary Cardholder Object with the SmartDataRecord values.</returns>
        private Domain.PrimaryCardholder PopulatePrimaryCardholderData(SmartDataRecord record)
        {
            var primaryCardholder = new Domain.PrimaryCardholder();

            if (!record.IsDBNull(ColumnCreditAccountId))
            {
                primaryCardholder.CreditAccountId = record.GetDecimal(ColumnCreditAccountId);
            }

            if (!record.IsDBNull(ColumnCreditBureauReportingFlag))
            {
                primaryCardholder.CreditBureauReportingFlag = record.GetString(ColumnCreditBureauReportingFlag);
            }

            if (!record.IsDBNull(ColumnCustomerId))
            {
                primaryCardholder.CustomerId = record.GetDecimal(ColumnCustomerId);
            }

            if (!record.IsDBNull(ColumnDateOfBirth))
            {
                primaryCardholder.DateOfBirth = record.GetDateTime(ColumnDateOfBirth);
            }

            if (!record.IsDBNull(ColumnNameId))
            {
                primaryCardholder.NameId = record.GetDecimal(ColumnNameId);
            }

            if (!record.IsDBNull(ColumnPrimaryCardholderdId))
            {
                primaryCardholder.Id = record.GetDecimal(ColumnPrimaryCardholderdId);
            }

            if (!record.IsDBNull(ColumnVirtualFirstName))
            {
                primaryCardholder.FirstName = record.GetString(ColumnVirtualFirstName);
            }

            if (!record.IsDBNull(ColumnVirtualLastName))
            {
                primaryCardholder.LastName = record.GetString(ColumnVirtualLastName);
            }

            return primaryCardholder;
        }

        #endregion
    }
}
