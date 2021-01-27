using CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Factory;
using CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Filter;

namespace CreditOne.Microservices.Account.Models
{
    /// <summary>
    /// Represents a criteria to search an account
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
    /// </item>
    /// <item>
    ///		<term>8/21/2020</term>
    ///		<term>Mauro Allende</term>
    ///		<term>RM-79</term>
    ///		<description>Refactor action methods</description>  
    /// </item>
    /// </list>
    /// </remarks> 
    public class AccountSearchModel
    {
        [ActionParameter("Ssn", EnumValidation.RegularExpression, Expression = "^$|^[0-9]{9}$",
            CustomErrorMessage = "Invalid SSN", Required = false)]
        public string Ssn { get; set; }
        public decimal? CustomerId { get; set; }
        public decimal? CreditAccountId { get; set; }
        public string CardNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public string CardPrefix { get; set; }
        public string CardType { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string Exactness { get; set; }
    }
}