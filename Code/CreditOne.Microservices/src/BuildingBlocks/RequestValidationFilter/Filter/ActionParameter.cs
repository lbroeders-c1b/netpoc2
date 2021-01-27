using System;

using CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Factory;

namespace CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Filter
{
    /// <summary>
    /// Represents the custom decotator to filter the parameters to be validate
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
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property,
        AllowMultiple = true, Inherited = false)]
    public class ActionParameter : Attribute
    {
        /// <summary>
        /// Name of the parameter to be validated
        /// </summary>
        public string ParamName { get; set; }
        /// <summary>
        /// Indicates if the parameter is required, true by default
        /// </summary>
        public bool Required { get; set; } = true;
        /// <summary>
        /// Regular expresion to be matched
        /// </summary>
        public string Expression { get; set; }
        /// <summary>
        /// Custom error message to be showed 
        /// </summary>
        public string CustomErrorMessage { get; set; }
        /// <summary>
        /// Indicates enumeration validation value
        /// </summary>
        public EnumValidation Validation { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="paramName">Parameter name</param>        
        public ActionParameter(string paramName)
        {
            this.ParamName = paramName;
            this.Validation = EnumValidation.Required;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="paramName">Parameter name</param>
        /// <param name="validation">Validation</param>
        public ActionParameter(string paramName, EnumValidation validation)
        {
            this.ParamName = paramName;
            this.Validation = validation;
        }
    }
}