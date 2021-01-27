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
    /// Implements the card oracle data provider class
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
    /// </list>
    /// </remarks>
    public class CardOracleDataProvider : OracleProviderBase, ICardDataProvider
    {
        #region Private Constants

        private const string DBConnectionStringKey = "DBConnectionString";

        private const string PkgCardProcedureGetForCreditAccount = "PKG_MS_CARD.PR_GET_BY_CREDIT_ACCOUNT";

        private const string ParameterCreditAccountId = "P_CREDIT_ACCOUNT_ID";
        private const string ParameterCardNumber = "P_CARD_NUMBER";

        private const string ColumnCardId = "CARD_ID";
        private const string ColumnCreditAccountId = "CREDIT_ACCOUNT_ID";
        private const string ColumnCurrentCard = "CURRENT_CARD";
        private const string ColumnExternalStatus = "EXTERNAL_STATUS";
        private const string ColumnInternalStatus = "INTERNAL_STATUS";
        private const string ColumnCardNumber = "CARD_NUM";
        private const string ColumnOldAccountId = "OLD_ACCOUNT_ID";
        private const string ColumnAbandonDate = "ABANDON_DATE";
        private const string ColumnPurgedFlag = "PURGED_FLAG";
        private const string ColumnStatusReason = "STATUS_REASON";
        private const string ColumnExpirationDate = "EXPIRATION_DATE";
        private const string ColumnPricingStrategyCode = "PRICING_STRATEGY_CODE";
        private const string ColumnDisputedFlag = "DISPUTED_FLAG";
        private const string ColumnPlasticDate = "PLASTIC_DT";
        private const string ColumnCurrentBalance = "BALANCE_CURRENT";
        private const string ColumnPrincipleBalance = "PRINCIPLE_BALANCE";
        private const string ColumnFraudTypeIndicator = "FRAUD_TYPE_INDICATOR";
        private const string ColumnLastMonetaryDate = "LAST_MONETARY_DATE";
        private const string ColumnTempCollectorCode = "TEMP_COLLECTOR_CODE";
        private const string ColumnActivationCode = "ACTIVATION_CODE";
        private const string ColumnCardType = "CARD_TYPE";
        private const string ColumnBillingDayOfMonth = "BILLING_DAY_OF_MONTH";
        private const string ColumnLostStolenDate = "LOST_STOLEN_DATE";
        private const string ColumnSysPrinAgentId = "SPA_ID";

        private const string CursorCards = "CR_CARDS";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>   
        public CardOracleDataProvider(DatabaseFunctions databaseFunctions,
                                      DbConnectionStrings dbConnectionStrings) :
            base(databaseFunctions, dbConnectionStrings)
        {
            ConnectionStringKey = DBConnectionStringKey;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns an historical record for all the cards associated to the credit account ID
        /// </summary>
        /// <param name="creditAccountId"></param>
        /// <returns>Task<List<Card>></returns>
        public async Task<IList<Domain.Card>> GetByCreditAccountId(decimal creditAccountId)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter(ParameterCreditAccountId, OracleDbType.Varchar2, creditAccountId, ParameterDirection.Input)
            };

            return await GetAsync(PkgCardProcedureGetForCreditAccount, parameters,
                            new OracleParameter(CursorCards, OracleDbType.RefCursor, ParameterDirection.Output), PopulateData);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Parses a database record for Card data.
        /// </summary>
        /// <param name="record">Database record for containing Card info</param>  
        /// <returns>The Card Object with the SmartDataRecord values.</returns>
        private Domain.Card PopulateData(SmartDataRecord record)
        {
            Domain.Card card = new Domain.Card();

            if (!record.IsDBNull(ColumnCardId))
            {
                card.Id = record.GetDecimal(ColumnCardId);
            }

            if (!record.IsDBNull(ColumnCreditAccountId))
            {
                card.CreditAccountId = record.GetDecimal(ColumnCreditAccountId);
            }

            if (!record.IsDBNull(ColumnCurrentCard))
            {
                card.IsCurrent = record.GetBoolean(ColumnCurrentCard);
            }

            if (!record.IsDBNull(ColumnExternalStatus))
            {
                card.ExternalStatusCode = record.GetString(ColumnExternalStatus);
            }

            if (!record.IsDBNull(ColumnInternalStatus))
            {
                card.InternalStatusCode = record.GetString(ColumnInternalStatus);
            }

            if (!record.IsDBNull(ColumnCardNumber))
            {
                card.FullCardNumber = record.GetString(ColumnCardNumber);
            }

            if (!record.IsDBNull(ColumnOldAccountId))
            {
                card.LegacyAccountId = record.GetDecimal(ColumnOldAccountId);
            }

            if (!record.IsDBNull(ColumnAbandonDate))
            {
                card.AbandonDate = record.GetDateTime(ColumnAbandonDate);
            }

            if (!record.IsDBNull(ColumnPurgedFlag))
            {
                card.IsPurged = record.GetBoolean(ColumnPurgedFlag);
            }

            if (!record.IsDBNull(ColumnStatusReason))
            {
                card.StatusReason = record.GetString(ColumnStatusReason);
            }

            if (!record.IsDBNull(ColumnExpirationDate))
            {
                card.ExpirationDate = record.GetDateTime(ColumnExpirationDate);
            }

            if (!record.IsDBNull(ColumnPricingStrategyCode))
            {
                card.PricingStrategyCode = record.GetString(ColumnPricingStrategyCode);
            }

            if (!record.IsDBNull(ColumnDisputedFlag))
            {
                card.IsInDispute = record.GetBoolean(ColumnDisputedFlag);
            }

            if (!record.IsDBNull(ColumnPlasticDate))
            {
                card.IssueDate = record.GetDateTime(ColumnPlasticDate);
            }

            if (!record.IsDBNull(ColumnCurrentBalance))
            {
                card.Balance = record.GetDecimal(ColumnCurrentBalance);
            }

            if (!record.IsDBNull(ColumnPrincipleBalance))
            {
                card.PrincipleBalance = record.GetDecimal(ColumnPrincipleBalance);
            }

            if (!record.IsDBNull(ColumnFraudTypeIndicator))
            {
                card.FraudTypeIndicator = record.GetString(ColumnFraudTypeIndicator);
            }

            if (!record.IsDBNull(ColumnLastMonetaryDate))
            {
                card.LastMonetaryDate = record.GetDateTime(ColumnLastMonetaryDate);
            }

            if (!record.IsDBNull(ColumnTempCollectorCode))
            {
                card.TempCollectorCode = record.GetString(ColumnTempCollectorCode);
            }

            if (!record.IsDBNull(ColumnActivationCode))
            {
                card.ActivationCode = record.GetString(ColumnActivationCode);
            }

            if (!record.IsDBNull(ColumnCardType))
            {
                card.TypeCode = record.GetChar(ColumnCardType);
            }

            if (!record.IsDBNull(ColumnBillingDayOfMonth))
            {
                card.BillingDayOfMonth = record.GetInt32(ColumnBillingDayOfMonth);
            }

            if (!record.IsDBNull(ColumnCardNumber))
            {
                card.FullCardNumber = record.GetString(ColumnCardNumber);
            }

            if (!record.IsDBNull(ColumnLostStolenDate))
            {
                card.LostStolenDate = record.GetDateTime(ColumnLostStolenDate);
            }

            if (!record.IsDBNull(ColumnSysPrinAgentId))
            {
                card.SysPrinAgentId = record.GetDecimal(ColumnSysPrinAgentId);
            }

            return card;
        }

        #endregion
    }
}
