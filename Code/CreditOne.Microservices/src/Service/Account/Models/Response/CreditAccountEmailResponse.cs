using System;

namespace CreditOne.Microservices.Account.Models.Response
{
    /// <summary>
    /// Implements the email response class
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
    ///	</item>
    /// </list>
    /// </remarks> 
    public class CreditAccountEmailResponse
    {
        public decimal? Id { get; set; }
        public decimal CreditAccountId { get; set; }
        public decimal? EmailId { get; set; }
        public string EmailTypeCode { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        public DateTime? VerifiedDateTime { get; set; }
        public string VerifCode { get; set; }
        public string EmailAddress { get; set; }
        public string EmailWoDomain { get; set; }
        public string StatusCode { get; set; }
    }
}
