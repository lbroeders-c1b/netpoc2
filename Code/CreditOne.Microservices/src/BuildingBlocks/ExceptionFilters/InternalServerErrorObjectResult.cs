using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CreditOne.Microservices.BuildingBlocks.ExceptionFilters
{
    /// <summary>
    /// Represents the internal server error object result
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
    ///     <term>4/03/2020</term>
    ///     <term>Christian Azula</term>
    ///     <term>RM-80</term>
    ///     <description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks>
    public class InternalServerErrorObjectResult : ObjectResult
    {
        /// <summary>
        /// Extension for InternalServerErrorObjectResult class
        /// </summary>
        /// <param name="error"></param>
        public InternalServerErrorObjectResult(object error) : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
