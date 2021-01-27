using System;
using System.Collections.Generic;

using CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Filter;

namespace CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Validators
{
    /// <summary>
    /// Represents the email validator class
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
    public class EmailValidator : RegularExpressionValidator, IValidate
    {
        private const string RegexEmail = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)"
                                        + @"|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        /// <summary>
        /// Verifies if a email string is a valid value
        /// </summary>
        /// <param name="actionArgumentValue">value to be validated</param>
        /// <param name="actionParameter">class public properties</param>
        /// <param name="errorResults">list of errors</param>
        /// <returns></returns>
        public new void IsValid(dynamic actionArgumentValue, ActionParameter actionParameter,
                                ref List<string> errorResults)
        {
            MatchExpression(Convert.ToString(actionArgumentValue), RegexEmail, actionParameter.ParamName,
                ref errorResults, actionParameter.CustomErrorMessage);
        }
    }
}