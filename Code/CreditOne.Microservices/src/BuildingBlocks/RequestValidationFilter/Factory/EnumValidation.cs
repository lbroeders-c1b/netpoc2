namespace CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Factory
{
    /// <summary>
    /// Enumeration to set the different kind of validation
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
    public enum EnumValidation
    {
        /// <summary>
        /// Makes the request argument value mandatory for the specified object. Null or empty are invalid values
        /// </summary>
        Required,
        /// <summary>
        /// Represents a validation of the type ID
        /// </summary>
        Id,
        /// <summary>
        /// Represents a validation of an email value
        /// </summary>
        Email,
        /// <summary>
        /// Represents a validation using a regular expression
        /// </summary>
        RegularExpression,
        /// <summary>
        /// Represents a validation of an Uri value
        /// </summary>
        Uri
    }
}