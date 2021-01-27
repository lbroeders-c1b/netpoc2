namespace CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Factory
{
    /// <summary>
    /// Defines validator factory interface
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
    public interface IValidatorFactory
    {
        /// <summary>
        /// Gets an instance of a concrete validator
        /// </summary>
        /// <param name="validation">enumeration validation value</param>
        /// <returns>an instance of a concrete validator</returns>
        IValidate GetValidator(EnumValidation validation);
    }
}