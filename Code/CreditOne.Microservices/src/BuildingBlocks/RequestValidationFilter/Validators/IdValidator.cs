using System;
using System.Collections.Generic;

using CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Filter;

namespace CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Validators
{
    /// <summary>
    /// Represents the identifier validator class
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
    public class IdValidator : IValidate
    {
        /// <summary>
        /// Verifies if an ID is a valid value
        /// </summary>
        /// <param name="actionArgumentValue">value to be validated</param>
        /// <param name="actionParameter">class public properties</param>
        /// <param name="errorResults">list of errors</param>
        /// <returns></returns>
        public void IsValid(dynamic actionArgumentValue, ActionParameter actionParameter, ref List<string> errorResults)
        {
            bool validationResult = true;

            validationResult &= this.IsGreaterThanOne(actionArgumentValue);

            validationResult &= !this.HasDecimalPart(actionArgumentValue);

            if (!validationResult)
            {
                string message;

                if (!string.IsNullOrEmpty(actionParameter.CustomErrorMessage))
                {
                    message = $"The paramater [{actionParameter.ParamName}] does not have a valid value: [{actionArgumentValue}]." +
                        $" {actionParameter.CustomErrorMessage}.";
                }
                else
                {
                    message = $"The parameter [{actionParameter.ParamName}] does not have a valid value: [{actionArgumentValue}]." +
                                     " The value must be equal or greater than 1 and must not include decimal value.";
                }

                errorResults.Add(message);
            }
        }

        /// <summary>
        /// Checks if a decimal value is equal or greater than 1
        /// </summary>
        /// <param name="value">value to be validated</param>
        /// <returns></returns>
        private bool IsGreaterThanOne(decimal value)
        {
            return value >= 1;
        }

        /// <summary>
        /// Checks if a decimal value has a decimal part
        /// </summary>
        /// <param name="value">value to be validated</param>
        /// <returns></returns>
        private bool HasDecimalPart(decimal value)
        {
            return (value - Math.Truncate(value)) > 0;
        }
    }
}
