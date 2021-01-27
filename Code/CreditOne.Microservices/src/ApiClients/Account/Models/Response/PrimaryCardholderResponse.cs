namespace CreditOne.Microservices.ApiClients.Account.Models.Response
{
    /// <summary>
    /// Implements the primary card holder response class
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
    public class PrimaryCardholderResponse : CardholderBaseResponse
    {
        public decimal CustomerId { get; set; }
        public string CreditBureauReportingFlag { get; set; }
        public bool IsCreditBureauReportingTriggered { get; }
        public bool HasBirthdayToday { get; }
    }
}
