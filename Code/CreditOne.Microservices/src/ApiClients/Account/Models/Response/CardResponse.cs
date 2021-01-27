using System;

namespace CreditOne.Microservices.ApiClients.Account.Models.Response
{
    /// <summary>
    /// Implements the card response class
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
    ///		<term>3/18/2020</term>
    ///		<term>Christian Azula</term>
    ///		<term>RM-80</term>
    ///		<description>Initial implementation</description>
    ///	</item>
    /// </list>
    /// </remarks> 
    public class CardResponse
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
