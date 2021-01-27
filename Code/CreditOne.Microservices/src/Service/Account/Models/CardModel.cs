using System;

namespace CreditOne.Microservices.Account.Models
{
    /// <summary>
    /// Implements the card model class
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
    ///		<description>Remove regions</description>  
    /// </item>
    /// </list>
    /// </remarks> 
    public class CardModel
    {
        public decimal Id { get; set; }
        public decimal CreditAccountId { get; set; }
        public string FullCardNumber { get; set; }
        public decimal SysPrinAgentId { get; set; }
        public decimal LegacyAccountId { get; set; }
        public bool IsCurrent { get; set; }
        public DateTime? AbandonDate { get; set; }
        public bool IsPurged { get; set; }
        public string ExternalStatusCode { get; set; }
        public string StatusReason { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string PricingStrategyCode { get; set; }
        public bool IsInDispute { get; set; }
        public DateTime IssueDate { get; set; }
        public decimal Balance { get; set; }
        public decimal PrincipleBalance { get; set; }
        public string FraudTypeIndicator { get; set; }
        public DateTime? LastMonetaryDate { get; set; }
        public string InternalStatusCode { get; set; }
        public string TempCollectorCode { get; set; }
        public string ActivationCode { get; set; }
        public DateTime? LostStolenDate { get; set; }
        public char TypeCode { get; set; }
        public int BillingDayOfMonth { get; set; }
        public string FormatCd { get; set; }
        public bool IsRealTimeAccount { get; set; }
        public string SystemOfRecord { get; set; }
    }
}
