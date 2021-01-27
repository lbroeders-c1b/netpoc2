using System;

namespace CreditOne.Microservices.ApiClients.Account.Models.Response
{
    /// <summary>
    /// Implements the cardholder base response class
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
    public class CardholderBaseResponse
    {
        public decimal Id { get; set; }
        public decimal CreditAccountId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public decimal NameId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int FdrRoleCode { get; set; }
    }
}
