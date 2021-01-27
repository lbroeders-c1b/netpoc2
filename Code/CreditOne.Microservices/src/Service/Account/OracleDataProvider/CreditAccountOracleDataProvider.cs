using System;
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
    /// Implements the credit account oracle data provider class
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
    public class CreditAccountOracleDataProvider : OracleProviderBase, ICreditAccountDataProvider
    {
        #region Private Constants

        private const string DBConnectionStringKey = "DBConnectionString";

        private const string PkgCreditAccountProcedureGetForCustomer = "PKG_MS_CREDIT_ACCOUNT.PR_GET_BY_CUSTOMER_ID";
        private const string PkgCreditAccountProcedureGetForCardNumber = "PKG_MS_CREDIT_ACCOUNT.PR_GET_BY_CARD_NUMBER";
        private const string PkgCreditAccountProcedureGetForId = "PKG_MS_CREDIT_ACCOUNT.PR_GET_BY_ID";

        private const string ParameterCardNumber = "P_CARD_NUMBER";
        private const string ParameterCustomerId = "P_CUSTOMER_ID";
        private const string ParameterCreditAccountId = "P_CREDIT_ACCOUNT_ID";

        private const string ColumnCreditAccountId = "CREDIT_ACCOUNT_ID";
        private const string ColumnCustomerId = "CUSTOMER_ID";
        private const string ColumnOpenDate = "OPEN_DATE";
        private const string ColumnAutoDepositFlag = "AUTO_DEPOSIT_FLAG";
        private const string ColumnBoardingCreditScore = "BOARDING_CREDIT_SCORE";
        private const string ColumnCreditBureauScore = "CREDIT_BUREAU_SCORE";
        private const string ColumnUpc12 = "UPC12";
        private const string ColumnRiskScore = "RISK_SCORE";
        private const string ColumnRandomDigits = "RANDOM_DIGITS";
        private const string ColumnBehaviorScore = "BEHAVIOR_SCORE";
        private const string ColumnMisc2 = "MISC_2";
        private const string ColumnPhoneFlag = "PHONE_FLAG";
        private const string ColumnAmmountLastPayment = "AMT_LAST_PAYMENT";
        private const string ColumnCreditLine = "CREDIT_LINE";
        private const string ColumnCreditLineChangeDate = "CREDIT_LINE_CHANGE_DATE";
        private const string ColumnCreditLineChangeType = "CREDIT_LINE_CHANGE_TYPE";
        private const string ColumnCyclesDelinquent = "CYCLES_DELINQUENT";
        private const string ColumnDateLastPayment = "DATE_LAST_PAYMENT";
        private const string ColumnDaysDelinguqent = "DAYS_DELINQUENT";
        private const string ColumnFixedPayment = "FIXED_PYMT";
        private const string ColumnHighBalance = "HIGH_BALANCE";
        private const string ColumnLastStatementDate = "LAST_STATEMENT_DATE";
        private const string ColumnLastStatementBalance = "LAST_STATEMENT_BALANCE";
        private const string ColumnPaymentDueDate = "PAYMENT_DUE_DATE";
        private const string ColumnPaymentHistoryCode = "PAYMENT_HISTORY_CODE";
        private const string ColumnReservationNumber = "RESERVATION_NUMBER";
        private const string ColumnSavingsAccountId = "SAVINGS_ACCOUNT_ID";
        private const string ColumnSourceCode = "SOURCE_CODE";
        private const string ColumnMmnPassword = "MMN_PASSWORD";
        private const string ColumnLanguageFlag = "LANGUAGE_FLAG";
        private const string ColumnAnnualChargeDate = "ANNUAL_CHARGE_DATE";
        private const string ColumnCloseZeroBalDate = "CLOSE_ZEROBAL_DATE";
        private const string ColumnCardId = "CARD_ID";
        private const string ColumnCardNumber = "CARD_NUM";
        private const string ColumnSpaId = "SPA_ID";
        private const string ColumnOldAccountId = "OLD_ACCOUNT_ID";
        private const string ColumnCurrentCard = "CURRENT_CARD";
        private const string ColumnAbandonDate = "ABANDON_DATE";
        private const string ColumnPurgedFlag = "PURGED_FLAG";
        private const string ColumnExternalStatus = "EXTERNAL_STATUS";
        private const string ColumnStatusReason = "STATUS_REASON";
        private const string ColumnExpirationDate = "EXPIRATION_DATE";
        private const string ColumnPricingStrategyCode = "PRICING_STRATEGY_CODE";
        private const string ColumnDisputedFlag = "DISPUTED_FLAG";
        private const string ColumnPlasticDt = "PLASTIC_DT";
        private const string ColumnBalanceCurrent = "BALANCE_CURRENT";
        private const string ColumnPrincipleBalance = "PRINCIPLE_BALANCE";
        private const string ColumnFraudTypeIndicator = "FRAUD_TYPE_INDICATOR";
        private const string ColumnLastMonetaryDate = "LAST_MONETARY_DATE";
        private const string ColumnInternalStatus = "INTERNAL_STATUS";
        private const string ColumnTempCollectorCode = "TEMP_COLLECTOR_CODE";
        private const string ColumnActivationCode = "ACTIVATION_CODE";
        private const string ColumnLostStolenDate = "LOST_STOLEN_DATE";
        private const string ColumnCardType = "CARD_TYPE";
        private const string ColumnBillingDayOfMonth = "BILLING_DAY_OF_MONTH";
        private const string ColumnFormatCd = "FORMAT_CD";
        private const string ColumnIsRealTimeAccount = "IS_REAL_TIME_ACCOUNT";
        private const string CursorCreditAccount = "CR_CREDIT_ACCOUNTS";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary> 
        public CreditAccountOracleDataProvider(DatabaseFunctions databaseFunctions,
                                               DbConnectionStrings dbConnectionStrings) :
            base(databaseFunctions, dbConnectionStrings)
        {
            ConnectionStringKey = DBConnectionStringKey;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the credit accounts for the given credit account ID
        /// </summary>
        /// <param name="creditAccountId">Credit account ID</param>
        /// <returns>List of credit accounts</returns>
        public async Task<IList<Domain.CreditAccount>> GetById(decimal creditAccountId)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter(ParameterCreditAccountId, OracleDbType.Varchar2, creditAccountId, ParameterDirection.Input)
            };

            return await GetAsync(PkgCreditAccountProcedureGetForId, parameters,
                         new OracleParameter(CursorCreditAccount, OracleDbType.RefCursor, ParameterDirection.Output), PopulateData);
        }

        /// <summary>
        /// Retrieves the credit accounts for the given customer ID
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>List of credit account</returns>
        public async Task<IList<Domain.CreditAccount>> GetByCustomerId(decimal customerId)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter(ParameterCustomerId, OracleDbType.Varchar2, customerId, ParameterDirection.Input)
            };

            return await GetAsync(PkgCreditAccountProcedureGetForCustomer, parameters,
                            new OracleParameter(CursorCreditAccount, OracleDbType.RefCursor, ParameterDirection.Output), PopulateData);
        }

        /// <summary>
        /// Retrieves the credit accounts for the given card number
        /// </summary>
        /// <param name="cardNumber">Card number</param>
        /// <returns>List of credit accounts</returns>
        public async Task<IList<Domain.CreditAccount>> GetByCardNumber(string cardNumber)
        {
            var parameters = new List<OracleParameter>
            {
                new OracleParameter(ParameterCardNumber, OracleDbType.Varchar2, cardNumber, ParameterDirection.Input)
            };

            return await GetAsync(PkgCreditAccountProcedureGetForCardNumber, parameters,
                         new OracleParameter(CursorCreditAccount, OracleDbType.RefCursor, ParameterDirection.Output), PopulateData);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Parses a database record for CreditAccount data.
        /// </summary>
        /// <param name="record">Database record for containing Credit Account info</param>  
        /// <returns>The Credit Account Object with the SmartDataRecord values.</returns>
        private Domain.CreditAccount PopulateData(SmartDataRecord record)
        {
            var creditAccount = new Domain.CreditAccount();

            if (!record.IsDBNull(ColumnCreditAccountId))
            {
                creditAccount.Id = record.GetDecimal(ColumnCreditAccountId);
            }

            if (!record.IsDBNull(ColumnCustomerId))
            {
                creditAccount.CustomerId = record.GetDecimal(ColumnCustomerId);
            }

            if (!record.IsDBNull(ColumnOpenDate))
            {
                creditAccount.OpenDate = record.GetDateTime(ColumnOpenDate);
            }

            if (!record.IsDBNull(ColumnAutoDepositFlag))
            {
                creditAccount.AutodepositFlags = record.GetBoolean(ColumnAutoDepositFlag);
            }

            if (!record.IsDBNull(ColumnBoardingCreditScore))
            {
                creditAccount.BoardingCreditScore = record.GetString(ColumnBoardingCreditScore);
            }

            if (!record.IsDBNull(ColumnCreditBureauScore))
            {
                creditAccount.CreditBureauScore = record.GetInt32(ColumnCreditBureauScore);
            }

            if (!record.IsDBNull(ColumnUpc12))
            {
                creditAccount.Upc12 = record.GetString(ColumnUpc12);
            }

            if (!record.IsDBNull(ColumnRiskScore))
            {
                creditAccount.RiskScore = record.GetInt32(ColumnRiskScore);
            }

            if (!record.IsDBNull(ColumnRandomDigits))
            {
                creditAccount.RandomDigits = record.GetInt32(ColumnRandomDigits);
            }

            if (!record.IsDBNull(ColumnBehaviorScore))
            {
                creditAccount.BehaviorScore = record.GetInt32(ColumnBehaviorScore);
            }

            if (!record.IsDBNull(ColumnMisc2))
            {
                creditAccount.Misc2 = record.GetString(ColumnMisc2);
            }

            if (!record.IsDBNull(ColumnPhoneFlag))
            {
                creditAccount.PhoneFlag = record.GetString(ColumnPhoneFlag);
            }

            if (!record.IsDBNull(ColumnAmmountLastPayment))
            {
                creditAccount.LastPaymentAmount = record.GetDecimal(ColumnAmmountLastPayment);
            }

            if (!record.IsDBNull(ColumnCreditLine))
            {
                creditAccount.CreditLine = record.GetDecimal(ColumnCreditLine);
            }

            if (!record.IsDBNull(ColumnCreditLineChangeDate))
            {
                creditAccount.CreditLineChangeDate = record.GetDateTime(ColumnCreditLineChangeDate);
            }

            if (!record.IsDBNull(ColumnCreditLineChangeType))
            {
                creditAccount.CreditLineChangeType = record.GetString(ColumnCreditLineChangeType);
            }

            if (!record.IsDBNull(ColumnCyclesDelinquent))
            {
                creditAccount.CyclesDelinquent = record.GetInt32(ColumnCyclesDelinquent);
            }

            if (!record.IsDBNull(ColumnDateLastPayment))
            {
                creditAccount.LastPaymentDate = record.GetDateTime(ColumnDateLastPayment);
            }

            if (!record.IsDBNull(ColumnDaysDelinguqent))
            {
                creditAccount.DaysDelinquent = record.GetInt32(ColumnDaysDelinguqent);
            }

            if (!record.IsDBNull(ColumnFixedPayment))
            {
                creditAccount.FixedPayment = record.GetInt32(ColumnFixedPayment);
            }

            if (!record.IsDBNull(ColumnHighBalance))
            {
                creditAccount.HighBalance = record.GetDecimal(ColumnHighBalance);
            }

            if (!record.IsDBNull(ColumnLastStatementDate))
            {
                creditAccount.LastStatementDate = record.GetDateTime(ColumnLastStatementDate);
            }

            if (!record.IsDBNull(ColumnLastStatementBalance))
            {
                creditAccount.LastStatementBalance = record.GetDecimal(ColumnLastStatementBalance);
            }

            if (!record.IsDBNull(ColumnPaymentDueDate))
            {
                creditAccount.PaymentDueDate = record.GetDateTime(ColumnPaymentDueDate);
            }

            if (!record.IsDBNull(ColumnPaymentHistoryCode))
            {
                creditAccount.PaymentHistoryCode = record.GetString(ColumnPaymentHistoryCode);
            }

            if (!record.IsDBNull(ColumnReservationNumber))
            {
                creditAccount.ReservationNumber = record.GetString(ColumnReservationNumber);
            }

            if (!record.IsDBNull(ColumnSavingsAccountId))
            {
                creditAccount.SavingsAccountId = record.GetDecimal(ColumnSavingsAccountId);
            }

            if (!record.IsDBNull(ColumnSourceCode))
            {
                creditAccount.SourceCode = record.GetString(ColumnSourceCode);
            }

            if (!record.IsDBNull(ColumnMmnPassword))
            {
                creditAccount.MmnPassword = record.GetString(ColumnMmnPassword);
            }

            if (!record.IsDBNull(ColumnLanguageFlag))
            {
                creditAccount.LanguageFlag = record.GetBoolean(ColumnLanguageFlag);
            }

            if (!record.IsDBNull(ColumnAnnualChargeDate))
            {
                creditAccount.AnnualChargeDate = record.GetString(ColumnAnnualChargeDate);
            }

            if (!record.IsDBNull(ColumnCloseZeroBalDate))
            {
                creditAccount.CloseZeroBalanceDate = record.GetDateTime(ColumnCloseZeroBalDate);
            }

            creditAccount.Card = PopulateCardData(record);

            return creditAccount;
        }

        /// <summary>
        /// Parses a database record for Card data.
        /// </summary>
        /// <param name="record">Database record for containing Card info</param>
        /// <returns>The Card Object with the SmartDataRecord values.</returns>
        private Domain.Card PopulateCardData(SmartDataRecord record)
        {
            var card = new Domain.Card();

            if (!record.IsDBNull(ColumnCardId))
            {
                card.Id = record.GetDecimal(ColumnCardId);
            }

            if (!record.IsDBNull(ColumnCreditAccountId))
            {
                card.CreditAccountId = record.GetDecimal(ColumnCreditAccountId);
            }

            if (!record.IsDBNull(ColumnCardNumber))
            {
                card.FullCardNumber = record.GetString(ColumnCardNumber);
            }

            if (!record.IsDBNull(ColumnSpaId))
            {
                card.SysPrinAgentId = record.GetDecimal(ColumnSpaId);
            }

            if (!record.IsDBNull(ColumnOldAccountId))
            {
                card.LegacyAccountId = record.GetDecimal(ColumnOldAccountId);
            }

            if (!record.IsDBNull(ColumnCurrentCard))
            {
                card.IsCurrent = record.GetBoolean(ColumnCurrentCard);
            }

            if (!record.IsDBNull(ColumnAbandonDate))
            {
                card.AbandonDate = record.GetDateTime(ColumnAbandonDate);
            }

            if (!record.IsDBNull(ColumnPurgedFlag))
            {
                card.IsPurged = record.GetBoolean(ColumnPurgedFlag);
            }

            if (!record.IsDBNull(ColumnExternalStatus))
            {
                card.ExternalStatusCode = record.GetString(ColumnExternalStatus);
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

            if (!record.IsDBNull(ColumnPlasticDt))
            {
                card.IssueDate = record.GetDateTime(ColumnPlasticDt);
            }

            if (!record.IsDBNull(ColumnBalanceCurrent))
            {
                card.Balance = record.GetDecimal(ColumnBalanceCurrent);
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

            if (!record.IsDBNull(ColumnInternalStatus))
            {
                card.InternalStatusCode = record.GetString(ColumnInternalStatus);
            }

            if (!record.IsDBNull(ColumnTempCollectorCode))
            {
                card.TempCollectorCode = record.GetString(ColumnTempCollectorCode);
            }

            if (!record.IsDBNull(ColumnActivationCode))
            {
                card.ActivationCode = record.GetString(ColumnActivationCode);
            }

            if (!record.IsDBNull(ColumnLostStolenDate))
            {
                card.LostStolenDate = record.GetDateTime(ColumnLostStolenDate);
            }

            if (!record.IsDBNull(ColumnCardType))
            {
                card.TypeCode = record.GetChar(ColumnCardType);
            }

            if (!record.IsDBNull(ColumnBillingDayOfMonth))
            {
                card.BillingDayOfMonth = record.GetInt32(ColumnBillingDayOfMonth);
            }

            if (!record.IsDBNull(ColumnFormatCd))
            {
                card.FormatCd = record.GetString(ColumnFormatCd);
            }

            if (!record.IsDBNull(ColumnIsRealTimeAccount))
            {
                card.IsRealTimeAccount = record.GetBoolean(ColumnIsRealTimeAccount);
            }

            return card;
        }

        #endregion
    }
}
