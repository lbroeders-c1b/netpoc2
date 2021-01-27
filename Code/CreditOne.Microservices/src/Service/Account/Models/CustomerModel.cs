namespace CreditOne.Microservices.Account.Models
{
    /// <summary>
    /// Implements the customer model class
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///     <term>Date</term>
    ///     <term>Who</term>
    ///     <term>BR/WO</term>
    ///     <description>Description</description>
    /// </listheader>
    /// <item>
    ///     <term>5/14/2019</term>
    ///     <term>Federico Bendayan</term>
    ///     <term>RM-47</term>
    ///     <description>Initial implementation</description>
    /// </item>
    /// <item>
    ///		<term>8/21/2020</term>
    ///		<term>Mauro Allende</term>
    ///		<term>RM-79</term>
    ///		<description>Remove regions</description>  
    /// </item>
    /// </list>
    /// </remarks> 	
    public class CustomerModel
    {
        public decimal Id { get; set; }
        public string Ssn { get; set; }
        public string ExperianPin { get; set; }
    }
}
