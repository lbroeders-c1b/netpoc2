using CreditOne.Microservices.BuildingBlocks.Types;

namespace CreditOne.Microservices.Account.Domain
{
    /// <summary>
    /// Implements the customer domain class
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///  <term>Date</term>
    ///  <term>Who</term>
    ///  <term>BR/WO</term>
    ///  <description>Description</description>
    /// </listheader>
    /// <item>
    ///  <term>4/11/2019</term>
    ///  <term>Federico Bendayan</term>
    ///  <term>RM-47</term>
    ///  <description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks>
    public class Customer
    {
        public decimal Id { get; set; }
        public string Ssn { get; set; }
        public SocialSecurityNumber SocialSecurityNumber => new SocialSecurityNumber(Ssn);
        public string ExperianPin { get; set; }
    }
}
