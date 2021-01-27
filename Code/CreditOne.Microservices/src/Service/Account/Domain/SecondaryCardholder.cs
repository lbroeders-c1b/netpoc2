namespace CreditOne.Microservices.Account.Domain
{
    /// <summary>
    /// Implements the secondary card holder domain class
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
	///		<term>10/3/2019</term>
	///		<term>Armando Soto</term>
	///		<term>Rm-47</term>
	///		<description>Initial implementation</description>
	/// </item>
    /// </list>
    /// </remarks> 	
    public class SecondaryCardholder : CardholderBase
    {
        public decimal SsnId { get; set; }
        public string Ssn { get; set; }
    }
}
