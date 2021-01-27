using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using CreditOne.Microservices.Account.Domain;
using CreditOne.Microservices.Account.IDataProvider;
using CreditOne.Microservices.BuildingBlocks.Common.Configuration;
using CreditOne.Microservices.BuildingBlocks.OracleProvider.Core;

using Oracle.ManagedDataAccess.Client;

namespace CreditOne.Microservices.Account.OracleDataProvider
{
    /// <summary>
    /// Implements the address oracle data provider class
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
    ///	</item>
    /// <item>
    ///		<term>8/21/2020</term>
    ///		<term>Mauro Allende</term>
    ///		<term>RM-79</term>
    ///		<description>Cleanup code</description>  
    /// </item>
    /// </list>
    /// </remarks>
    public class AddressOracleDataProvider : OracleProviderBase, IAddressDataProvider
    {
        #region Private Constants

        private const string DBConnectionStringKey = "DBConnectionString";

        private const string PkgAddressProcedureGetAddressByCreditAccountId = "PKG_MS_ADDRESS.PR_GET_BY_CREDIT_ACCOUNT_ID";

        private const string ParameterCreditAccountId = "P_CREDIT_ACCOUNT_ID";

        private const string CursorAddresses = "CR_ADDRESSES";

        private const string ColumnAddressId = "ADDRESS_ID";
        private const string ColumnAddressLineOne = "ADDRESS_LINE1";
        private const string ColumnAddressLineTwo = "ADDRESS_LINE2";
        private const string ColumnCity = "CITY";
        private const string ColumnState = "STATE";
        private const string ColumnZip = "ZIP";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>   
        public AddressOracleDataProvider(DatabaseFunctions databaseFunctions,
                                         DbConnectionStrings dbConnectionStrings) :
            base(databaseFunctions, dbConnectionStrings)
        {
            ConnectionStringKey = DBConnectionStringKey;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the addresses associated to the credit account ID
        /// </summary>
        /// <param name="creditAccountId"></param>
        /// <returns>List<Address></returns>
        public async Task<IList<Address>> GetByCreditAccountId(decimal creditAccountId)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter(ParameterCreditAccountId, OracleDbType.Decimal, creditAccountId, ParameterDirection.Input)
            };

            return await GetAsync(PkgAddressProcedureGetAddressByCreditAccountId, parameters,
                            new OracleParameter(CursorAddresses, OracleDbType.RefCursor, ParameterDirection.Output), PopulateData);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Parses a database record for Address data
        /// </summary>
        /// <param name="record">Database record for containing Address info</param>  
        /// <returns>The Address Object with the SmartDataRecord values.</returns>
        private Address PopulateData(SmartDataRecord record)
        {
            Address address = new Address();

            if (!record.IsDBNull(ColumnAddressId))
            {
                address.Id = record.GetDecimal(ColumnAddressId);
            }

            if (!record.IsDBNull(ColumnAddressLineOne))
            {
                address.AddressLineOne = record.GetString(ColumnAddressLineOne);
            }

            if (!record.IsDBNull(ColumnAddressLineTwo))
            {
                address.AddressLineTwo = record.GetString(ColumnAddressLineTwo);
            }

            if (!record.IsDBNull(ColumnCity))
            {
                address.City = record.GetString(ColumnCity);
            }

            if (!record.IsDBNull(ColumnState))
            {
                address.State = record.GetString(ColumnState);
            }

            if (!record.IsDBNull(ColumnZip))
            {
                address.Zip = record.GetString(ColumnZip);
            }

            return address;
        }

        #endregion
    }
}
