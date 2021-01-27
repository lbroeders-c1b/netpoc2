using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Filter;

namespace CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Validators
{
    /// <summary>
    /// Represents the regular expresion validator class
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
    public class RegularExpressionValidator : IValidate
    {
        /// <summary>
        /// Verifies if a string is a valid value
        /// </summary>
        /// <param name="actionArgumentValue">value to be validated</param>
        /// <param name="actionParameter">class public properties</param>        
        /// <param name="errorResults">list of errors</param>
        /// <returns></returns>
        public void IsValid(dynamic actionArgumentValue, ActionParameter actionParameter, ref List<string> errorResults)
        {
            MatchExpression(actionArgumentValue, actionParameter.Expression, actionParameter.ParamName, ref errorResults,
                actionParameter.CustomErrorMessage);
        }

        /// <summary>
        /// Matchs a regular expresion with a string value
        /// </summary>
        /// <param name="actionArgumentValue">value to be validated</param>
        /// <param name="regularExpresion">regular expression</param>
        /// <param name="propertyName">name of the property to be validated</param>
        /// <param name="errorResults">list of errors</param>
        /// <param name="customErrorMessage">error message</param>
        /// <returns></returns>
        protected void MatchExpression(string actionArgumentValue, string regularExpresion,
                                       string propertyName, ref List<string> errorResults, string customErrorMessage)
        {
            var regex = new Regex(regularExpresion, RegexOptions.IgnoreCase);

            if (!regex.Matches(actionArgumentValue).Any())
            {
                string message;

                if (!string.IsNullOrEmpty(customErrorMessage))
                {
                    message = $"The paramater [{propertyName}] is not a valid expression. {customErrorMessage}.";
                }
                else
                {
                    message = $"The parameter [{propertyName}] is not a valid expression " +
                                     $"[{regularExpresion}] value: [{actionArgumentValue}].";
                }

                errorResults.Add(message);
            }
        }
    }
}
