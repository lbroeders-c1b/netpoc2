using System.Collections.Generic;
using System.Threading.Tasks;

namespace CreditOne.Microservices.Account.IDataProvider
{
    /// <summary>
    /// Defines the account data provider interface
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
    ///     <term>5/30/2019</term>
    ///     <term>Federico Bendayan</term>
    ///     <term>RM-47</term>
    ///     <description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks>
    public interface IAccountDataProvider
    {
        /// <summary>
        /// Returns a list of accounts using the parameters as filters
        /// </summary>
        /// <param name="homePhone">Home phone</param>
        /// <param name="workPhone">Work phone</param>
        /// <param name="lastName">Last name</param>
        /// <param name="firstName">First name</param>
        /// <param name="middleInitial">Middle initial</param>
        /// <param name="cardPrefix">Card prefix</param>
        /// <param name="cardType">Card type</param>
        /// <param name="state">State</param>
        /// <param name="zip">Zip</param>
        /// <returns>List of accounts</returns>
        Task<IList<Domain.Account>> GetByCustomerDemographics(string homePhone, string workPhone, string lastName, string firstName,
                                                              string middleInitial, string cardPrefix, string cardType, string state,
                                                              string zip, string exactnessValueForBoats);

        /// <summary>
        /// Returns a list of accounts using the parameters as filters
        /// </summary>
        /// <param name="addressLineOne">Address line one</param>
        /// <param name="addressLineTwo">Address line two</param>
        /// <param name="city">City</param>
        /// <param name="state">State</param>
        /// <param name="zip">Zip</param>
        /// <returns>List of accounts</returns>
        Task<IList<Domain.Account>> GetByCustomerDemographics(string addressLineOne, string addressLineTwo, string city, string state,
                                                              string zip);
    }
}
