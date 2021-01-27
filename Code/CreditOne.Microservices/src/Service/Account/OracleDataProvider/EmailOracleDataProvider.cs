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
    /// Implements the email oracle data provider class
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
    public class EmailOracleDataProvider : OracleProviderBase, IEmailDataProvider
    {
        #region Private Constants

        private const string DBConnectionStringKey = "DBConnectionString";

        private const string PkgEmailProcedureGetForCreditAccount = "PKG_MS_CREDIT_ACCOUNT_EMAIL.PR_GET_BY_CREDIT_ACCOUNT";

        private const string ParameterCreditAccountId = "P_CREDIT_ACCOUNT_ID";

        private const string CursorEmails = "CR_ACCOUNT_EMAILS";

        private const string ColumnAccountEmailId = "ACCOUNT_EMAIL_ID";
        private const string ColumnEmailAddressId = "EMAIL_ADDRESS_ID";
        private const string ColumnEmailTypeCode = "EMAIL_TYPE_CODE";
        private const string ColumnCreditAccountId = "CREDIT_ACCOUNT_ID";
        private const string ColumnDateModified = "DATE_MODIFIED";
        private const string ColumnVerifCode = "VERIF_CODE";
        private const string ColumnVerifiedDate = "VERIFIED_DATE";
        private const string ColumnEmailAddress = "EMAIL_ADDRESS";
        private const string ColumnEmailWoDomain = "EMAIL_WO_DOMAIN";
        private const string ColumnStatusCode = "STATUS_CODE";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>   
        public EmailOracleDataProvider(DatabaseFunctions databaseFunctions, 
                                       DbConnectionStrings dbConnectionStrings) : 
            base(databaseFunctions, dbConnectionStrings)
        {
            ConnectionStringKey = DBConnectionStringKey;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the associated emails to the credit account ID
        /// </summary>
        /// <param name="creditAccountId"></param>
        /// <returns>Task<List<Domain.CreditAccountEmail>></returns>
        public async Task<IList<Domain.CreditAccountEmail>> GetByCreditAccountId(decimal creditAccountId)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter(ParameterCreditAccountId, OracleDbType.Varchar2, creditAccountId, ParameterDirection.Input)
            };

            return await GetAsync(PkgEmailProcedureGetForCreditAccount, parameters,
                            new OracleParameter(CursorEmails, OracleDbType.RefCursor, ParameterDirection.Output), PopulateData);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Parses a database record for Email data.
        /// </summary>
        /// <param name="record">Database record for containing Email info</param>  
        /// <returns>The Email Object with the SmartDataRecord values.</returns>
        private Domain.CreditAccountEmail PopulateData(SmartDataRecord record)
        {
            Domain.CreditAccountEmail email = new Domain.CreditAccountEmail();

            if (!record.IsDBNull(ColumnAccountEmailId))
            {
                email.Id = record.GetDecimal(ColumnAccountEmailId);
            }

            if (!record.IsDBNull(ColumnEmailAddressId))
            {
                email.EmailId = record.GetDecimal(ColumnEmailAddressId);
            }

            if (!record.IsDBNull(ColumnEmailTypeCode))
            {
                email.EmailTypeCode = record.GetString(ColumnEmailTypeCode);
            }

            if (!record.IsDBNull(ColumnCreditAccountId))
            {
                email.CreditAccountId = record.GetDecimal(ColumnCreditAccountId);
            }

            if (!record.IsDBNull(ColumnDateModified))
            {
                email.LastModifiedDateTime = record.GetDateTime(ColumnDateModified);
            }

            if (!record.IsDBNull(ColumnVerifCode))
            {
                email.VerifCode = record.GetString(ColumnVerifCode);
            }

            if (!record.IsDBNull(ColumnEmailAddress))
            {
                email.EmailAddress = record.GetString(ColumnEmailAddress);
            }

            if (!record.IsDBNull(ColumnEmailWoDomain))
            {
                email.EmailWoDomain = record.GetString(ColumnEmailWoDomain);
            }

            if (!record.IsDBNull(ColumnStatusCode))
            {
                email.StatusCode = record.GetString(ColumnStatusCode);
            }

            return email;
        }

        #endregion
    }
}
