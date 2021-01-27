using CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Validators;

namespace CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Factory
{
    /// <summary>
    /// Represents the validator factory class
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
    public class ValidatorFactory : IValidatorFactory
    {
        /// <summary>
        /// Gets an instance of a concrete validator
        /// </summary>
        /// <param name="validation">Enumeration validation value</param>
        /// <returns>Instance of a concrete validator</returns>
        public IValidate GetValidator(EnumValidation validation)
        {
            switch (validation)
            {
                case EnumValidation.Id:
                    return new IdValidator();

                case EnumValidation.Email:
                    return new EmailValidator();

                case EnumValidation.RegularExpression:
                    return new RegularExpressionValidator();

                case EnumValidation.Uri:
                    return new UriValidator();

                default:
                    return null;
            }
        }
    }
}