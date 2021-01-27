using System;

using CreditOne.Microservices.BuildingBlocks.Types;

namespace CreditOne.Microservices.Account.Domain
{
    /// <summary>
    /// Implements the card holder base domain class
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
    ///		<term>Christian Azula</term>
    ///		<term>RM-47</term>
    ///		<description>Initial implementation</description>
    ///	</item>
    /// </list>
    /// </remarks> 
    public class CardholderBase
    {
        public decimal Id { get; set; }
        public decimal CreditAccountId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public decimal NameId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public PersonName PersonName => new PersonName(FirstName, LastName);
    }
}