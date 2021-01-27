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
    /// Implements the customer oracle data provider class
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///     <term>Date</term>
    ///     <term>Who</term>
    ///     <term>BR/WO</term>
    ///     <description>Description</description>
    /// </listheader>
    /// <item>
    ///     <term>4/12/2019</term>
    ///     <term>Federico Bendayan</term>
    ///     <term>RM-47</term>
    ///     <description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks>
    public class CustomerOracleDataProvider : OracleProviderBase, ICustomerDataProvider
    {
        #region Private Constants

        private const string DBConnectionStringKey = "DBConnectionString";

        private const string PkgCustomerProcedureGetCustomerForSsn = "PKG_MS_CUSTOMER.PR_GET_ID_BY_SSN";
        private const string PkgCustomerProcedureGetCustomerForId = "PKG_MS_CUSTOMER.PR_GET_BY_ID";

        private const string ParameterSSN = "P_SSN";
        private const string ParameterCustomerId = "P_CUSTOMER_ID";

        private const string CursorReturn = "P_REF_CURSOR";

        private const string ColumnCustomerId = "CUSTOMER_ID";
        private const string ColumnSSN = "SSN";
        private const string ColumnLastFourSSN = "LAST4_SSN";
        private const string ColumnExperianPin = "EXPERIAN_PIN";
        private const string ColumnCreditAccountId = "CREDIT_ACCOUNT_ID";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>   
        public CustomerOracleDataProvider(DatabaseFunctions databaseFunctions,
                                          DbConnectionStrings dbConnectionStrings) :
            base(databaseFunctions, dbConnectionStrings)
        {
            ConnectionStringKey = DBConnectionStringKey;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the customers associated to the given social security number
        /// </summary>
        /// <param name="ssn">Social security number</param>
        /// <returns>List of customers</returns>
        public async Task<IList<Domain.Customer>> GetBySSN(string ssn)
        {
            var parameters = new List<OracleParameter>
                {
                    new OracleParameter(ParameterSSN, OracleDbType.Varchar2, ssn, ParameterDirection.Input)
                };

            return await GetAsync(PkgCustomerProcedureGetCustomerForSsn, parameters,
                            new OracleParameter(CursorReturn, OracleDbType.RefCursor, ParameterDirection.Output), PopulateData);
        }

        /// <summary>
        /// Retrieves the customer associated to the given customer ID
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>List of customers</returns>
        public async Task<IList<Domain.Customer>> GetById(decimal customerId)
        {
            var parameters = new List<OracleParameter>
                {
                    new OracleParameter(ParameterCustomerId, OracleDbType.Varchar2, customerId, ParameterDirection.Input)
                };

            return await GetAsync(PkgCustomerProcedureGetCustomerForId, parameters,
                            new OracleParameter(CursorReturn, OracleDbType.RefCursor, ParameterDirection.Output), PopulateData);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Parses a database record for customer data.
        /// </summary>
        /// <param name="record">Database record for containing customer info</param>  
        /// <returns>The customer Object with the SmartDataRecord values.</returns>
        private Domain.Customer PopulateData(SmartDataRecord record)
        {
            Domain.Customer customer = new Domain.Customer();

            if (!record.IsDBNull(ColumnCustomerId))
            {
                customer.Id = record.GetDecimal(ColumnCustomerId);
            }

            if (!record.IsDBNull(ColumnSSN))
            {
                customer.Ssn = record.GetString(ColumnSSN);
            }

            if (!record.IsDBNull(ColumnExperianPin))
            {
                customer.ExperianPin = record.GetString(ColumnExperianPin);
            }

            return customer;
        }

        #endregion
    }
}
