namespace CreditOne.Microservices.Account.Models
{
    /// <summary>
    /// Implements the account model class
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
    ///		<term>5/29/2019</term>
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
    public class AccountModel
    {
        public string PrimaryName { get; set; }
        public string SecondaryName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string InternalStatusCode { get; set; }
        public string ExternalStatus { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string SystemOfRecord { get; set; }
        public string Last4SSN { get; set; }
        public decimal CreditAccountId { get; set; }
        public decimal CustomerId { get; set; }
        public string CardNumber { get; set; }
    }
}
