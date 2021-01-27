namespace CreditOne.Microservices.ApiClients.Account.Models.Request
{
    /// <summary>
    /// Implements the address request class
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
    ///		<term>3/28/2020</term>
    ///		<term>Christian Azula</term>
    ///		<term>RM-80</term>
    ///		<description>Initial implementation</description>
    ///	</item>
    /// </list>
    /// </remarks> 
    public class AddressRequest
    {
        public decimal Id { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}
