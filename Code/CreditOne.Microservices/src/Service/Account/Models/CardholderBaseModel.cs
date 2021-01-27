using System;

namespace CreditOne.Microservices.Account.Models
{
    /// <summary>
    /// Implements the card holder base model class
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
    ///		<term>5/30/2019</term>
    ///		<term>Christian Azula</term>
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
    public class CardholderBaseModel
    {
        public decimal Id { get; set; }
        public decimal CreditAccountId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public decimal NameId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}
