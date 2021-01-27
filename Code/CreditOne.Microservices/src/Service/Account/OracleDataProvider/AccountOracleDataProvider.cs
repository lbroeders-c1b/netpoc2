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
    /// Implements the account oracle data provider class
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///	 <term>Date</term>
    ///	 <term>Who</term>
    ///	 <term>BR/WO</term>
    ///	 <description>Description</description>
    /// </listheader>
	/// <item>
	///	 <term>8/12/2019</term>
	///	 <term>Federico Bendayan</term>
	///	 <term>RM-47</term>
	///	 <description>Initial implementation</description>
	/// </item>
    /// </list>
    /// </remarks> 	
    public class AccountOracleDataProvider : OracleProviderBase, IAccountDataProvider
    {
        #region Private Constants

        private const string DBConnectionStringKey = "DBConnectionString";

        private const string PkgCustomerProcedureAccountSearch = "PKG_MS_ACCOUNT.PR_ACCOUNT_SEARCH";

        private const string ParameterHomePhone = "P_HOME_PHONE";
        private const string ParameterWorkPhone = "P_WORK_PHONE";
        private const string ParameterLastName = "P_LAST_NAME";
        private const string ParameterFirstName = "P_FIRST_NAME";
        private const string ParameterMiddleInitial = "P_MIDD_INITIAL";
        private const string ParameterCardPrefix = "P_CARD_PREFIX";
        private const string ParameterCardType = "P_CARD_TYPE";
        private const string ParameterState = "P_STATE";
        private const string ParameterZip = "P_ZIP";
        private const string ParameterCity = "P_CITY";
        private const string ParameterAddressLineOne = "P_ADDLINE1";
        private const string ParameterAddressLineTwo = "P_ADDLINE2";
        private const string ParameterExactness = "P_EXACTNESS";

        private const string ColumnId = "SESSION_ID";
        private const string ColumnPrimaryName = "PRIMARY_NAME";
        private const string ColumnSecondaryName = "SECONDARY_NAME";
        private const string ColumnAddressLine1 = "ADDRESS_LINE1";
        private const string ColumnAddressLine2 = "ADDRESS_LINE2";
        private const string ColumnInternalStatusCode = "INTERNAL_STATUS_CODE";
        private const string ColumnExternalStatus = "EXTERNAL_STATUS";
        private const string ColumnCity = "CITY";
        private const string ColumnState = "STATE";
        private const string ColumnZip = "ZIP";
        private const string ColumnSystemOfRecord = "SYSTEM_OF_RECORD";
        private const string ColumnAccountSearchStageId = "T_ACCOUNT_SEARCH_STG_ID";
        private const string ColumnLast4SSN = "LAST4_SSN";
        private const string ColumnCreditAccountId = "CREDIT_ACCOUNT_ID";
        private const string ColumnCustomerId = "CUSTOMER_ID";
        private const string ColumnCardId = "CARD_ID";
        private const string ColumnCarNumber = "CAR_NUMBER";
        private const string ColumnCardNumber = "CARD_NUMBER";
        private const string CursorReturn = "P_RETURN";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>   
        public AccountOracleDataProvider(DatabaseFunctions databaseFunctions,
                                         DbConnectionStrings dbConnectionStrings) :
            base(databaseFunctions, dbConnectionStrings)
        {
            ConnectionStringKey = DBConnectionStringKey;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a list of customer found using the parameters as filters
        /// </summary>
        /// <param name="homePhone"></param>
        /// <param name="workPhone"></param>
        /// <param name="lastName"></param>
        /// <param name="firstName"></param>
        /// <param name="middleInitial"></param>
        /// <param name="cardPrefix"></param>
        /// <param name="cardType"></param>
        /// <param name="state"></param>
        /// <param name="zip"></param>
        /// <returns>List of <Domain.Customer></returns>
        public async Task<IList<Domain.Account>> GetByCustomerDemographics(string homePhone, string workPhone,
                                                                           string lastName, string firstName,
                                                                           string middleInitial, string cardPrefix,
                                                                           string cardType, string state,
                                                                           string zip, string exactnessValueForBoats)
        {
            var parameters = new List<OracleParameter>
                {
                    new OracleParameter(ParameterHomePhone, OracleDbType.Varchar2, homePhone, ParameterDirection.Input),
                    new OracleParameter(ParameterWorkPhone, OracleDbType.Varchar2, workPhone, ParameterDirection.Input),
                    new OracleParameter(ParameterLastName, OracleDbType.Varchar2, lastName, ParameterDirection.Input),
                    new OracleParameter(ParameterFirstName, OracleDbType.Varchar2, firstName, ParameterDirection.Input),
                    new OracleParameter(ParameterMiddleInitial, OracleDbType.Varchar2, middleInitial, ParameterDirection.Input),
                    new OracleParameter(ParameterCardPrefix, OracleDbType.Varchar2, cardPrefix, ParameterDirection.Input),
                    new OracleParameter(ParameterCardType, OracleDbType.Varchar2, cardType, ParameterDirection.Input),
                    new OracleParameter(ParameterState, OracleDbType.Varchar2, state, ParameterDirection.Input),
                    new OracleParameter(ParameterZip, OracleDbType.Varchar2, zip, ParameterDirection.Input),
                    new OracleParameter(ParameterExactness, OracleDbType.Varchar2, exactnessValueForBoats , ParameterDirection.Input)
                };

            return await GetAsync(PkgCustomerProcedureAccountSearch, parameters,
                            new OracleParameter(CursorReturn, OracleDbType.RefCursor, ParameterDirection.Output), PopulateData);
        }

        /// <summary>
        /// Returns a list of customer found using the parameters as filters
        /// </summary>
        /// <param name="addressLineOne"></param>
        /// <param name="addressLineTwo"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="zip"></param>
        /// <returns>IList<Domain.Customer></returns>
        public async Task<IList<Domain.Account>> GetByCustomerDemographics(string addressLineOne, string addressLineTwo,
                                                                           string city, string state, string zip)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter(ParameterAddressLineOne, OracleDbType.Varchar2, addressLineOne, ParameterDirection.Input),
                new OracleParameter(ParameterAddressLineTwo, OracleDbType.Varchar2, addressLineTwo, ParameterDirection.Input),
                new OracleParameter(ParameterCity, OracleDbType.Varchar2, city, ParameterDirection.Input),
                new OracleParameter(ParameterState, OracleDbType.Varchar2, state, ParameterDirection.Input),
                new OracleParameter(ParameterZip, OracleDbType.Varchar2, zip, ParameterDirection.Input)
            };

            return await GetAsync(PkgCustomerProcedureAccountSearch, parameters,
                    new OracleParameter(CursorReturn, OracleDbType.RefCursor, ParameterDirection.Output), PopulateData);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Parses a database record for customer data.
        /// </summary>
        /// <param name="record">Database record for containing customer info</param>  
        /// <returns>The customer Object with the SmartDataRecord values.</returns>
        private Domain.Account PopulateData(SmartDataRecord record)
        {
            var account = new Domain.Account();

            if (!record.IsDBNull(ColumnAddressLine1))
            {
                account.AddressLine1 = record.GetString(ColumnAddressLine1);
            }

            if (!record.IsDBNull(ColumnAddressLine2))
            {
                account.AddressLine2 = record.GetString(ColumnAddressLine2);
            }

            if (!record.IsDBNull(ColumnCardNumber))
            {
                account.CardNumber = record.GetString(ColumnCardNumber);
            }

            if (!record.IsDBNull(ColumnCity))
            {
                account.City = record.GetString(ColumnCity);
            }

            if (!record.IsDBNull(ColumnCreditAccountId))
            {
                account.CreditAccountId = record.GetDecimal(ColumnCreditAccountId);
            }

            if (!record.IsDBNull(ColumnCustomerId))
            {
                account.CustomerId = record.GetDecimal(ColumnCustomerId);
            }

            if (!record.IsDBNull(ColumnExternalStatus))
            {
                account.ExternalStatus = record.GetString(ColumnExternalStatus);
            }

            if (!record.IsDBNull(ColumnInternalStatusCode))
            {
                account.InternalStatusCode = record.GetString(ColumnInternalStatusCode);
            }

            if (!record.IsDBNull(ColumnLast4SSN))
            {
                account.Last4SSN = record.GetString(ColumnLast4SSN);
            }

            if (!record.IsDBNull(ColumnPrimaryName))
            {
                account.PrimaryName = record.GetString(ColumnPrimaryName);
            }

            if (!record.IsDBNull(ColumnSecondaryName))
            {
                account.SecondaryName = record.GetString(ColumnSecondaryName);
            }

            if (!record.IsDBNull(ColumnState))
            {
                account.State = record.GetString(ColumnState);
            }

            if (!record.IsDBNull(ColumnSystemOfRecord))
            {
                account.SystemOfRecord = record.GetString(ColumnSystemOfRecord);
            }

            if (!record.IsDBNull(ColumnZip))
            {
                account.Zip = record.GetString(ColumnZip);
            }

            return account;
        }

        #endregion
    }
}
