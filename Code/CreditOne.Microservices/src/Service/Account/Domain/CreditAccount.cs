using System;

namespace CreditOne.Microservices.Account.Domain
{
    /// <summary>
    /// Implements the credit account domain class
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
    public class CreditAccount
    {
        public decimal Id { get; set; }
        public decimal CustomerId { get; set; }
        public DateTime? OpenDate { get; set; }
        public bool AutodepositFlags { get; set; }
        public string BoardingCreditScore { get; set; }
        public int CreditBureauScore { get; set; }
        public string Upc12 { get; set; }
        public int RiskScore { get; set; }
        public int RandomDigits { get; set; }
        public int BehaviorScore { get; set; }
        public string Misc2 { get; set; }
        public string PhoneFlag { get; set; }
        public decimal LastPaymentAmount { get; set; }
        public decimal CreditLine { get; set; }
        public DateTime? CreditLineChangeDate { get; set; }
        public string CreditLineChangeType { get; set; }
        public int CyclesDelinquent { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public int DaysDelinquent { get; set; }
        public int FixedPayment { get; set; }
        public decimal HighBalance { get; set; }
        public DateTime? LastStatementDate { get; set; }
        public decimal? LastStatementBalance { get; set; }
        public DateTime? PaymentDueDate { get; set; }
        public string PaymentHistoryCode { get; set; }
        public string ReservationNumber { get; set; }
        public decimal SavingsAccountId { get; set; }
        public string SourceCode { get; set; }
        public string StatementHoldCode { get; set; }
        public string MmnPassword { get; set; }
        public bool LanguageFlag { get; set; }
        public string AnnualChargeDate { get; set; }
        public DateTime? CloseZeroBalanceDate { get; set; }
        public Card Card { get; set; }

        public bool HasBadPhone
        {
            get
            {
                return PhoneFlag == "Y";
            }
        }
    }
}
