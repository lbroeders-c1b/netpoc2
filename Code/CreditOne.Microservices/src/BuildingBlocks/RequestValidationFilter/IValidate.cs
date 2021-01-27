using System.Collections.Generic;

using CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Filter;

namespace CreditOne.Microservices.BuildingBlocks.RequestValidationFilter
{
    /// <summary>    
    /// Defines validate interface    
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
    public interface IValidate
    {
        /// <summary>
        /// Verifies if a value is valid
        /// </summary>
        /// <param name="actionArgumentValue">value to be validated</param>
        /// <param name="actionParameter">class public properties</param>
        /// <param name="errorResults">list of errors</param>
        /// <returns></returns>
        void IsValid(dynamic actionArgumentValue, ActionParameter actionParameter, ref List<string> errorResults);
    }
}
