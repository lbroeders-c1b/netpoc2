using System;
using System.Collections.Generic;
using System.Text;
using CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Filter;

namespace CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Validators
{
    /// <summary>
    /// Represents the Uri validator class
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
    ///		<term>6/23/2020</term>
    ///		<term>Levi Tamiozzo</term>
    ///		<term>MK-1001</term>
    ///		<description>Initial implementation</description>
    ///	</item>
    /// </list>
    /// </remarks> 
    public class UriValidator : IValidate
    {        
        /// <summary>
        /// Verifies if an Uri string is a valid value
        /// </summary>
        /// <param name="actionArgumentValue">value to be validated</param>
        /// <param name="actionParameter">class public properties</param>
        /// <param name="errorResults">list of errors</param>
        /// <returns></returns>
        public void IsValid(dynamic actionArgumentValue, ActionParameter actionParameter,
                                ref List<string> errorResults)
        {
            bool validationResult = Uri.IsWellFormedUriString(actionArgumentValue, UriKind.Absolute);

            if (!validationResult)
            {
                string message;

                if (!string.IsNullOrEmpty(actionParameter.CustomErrorMessage))
                {
                    message = $"The parameter [{actionParameter.ParamName}] does not have a valid value: [{actionArgumentValue}]." +
                              $" {actionParameter.CustomErrorMessage}.";
                }
                else
                {
                    message = $"The parameter [{actionParameter.ParamName}] does not have a valid value. The value must be a valid absolute Uri.";
                }

                errorResults.Add(message);
            }
        }
    }
}
