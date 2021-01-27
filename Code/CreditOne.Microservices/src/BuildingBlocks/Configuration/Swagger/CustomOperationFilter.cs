using System.Linq;

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CreditOne.Microservices.BuildingBlocks.Common.Configuration.Swagger
{
    /// <summary>
    /// Represents the configuration of URIs 
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///     <term>Date</term>
    ///     <term>Who</term>
    ///     <term>BR/WO</term>
    ///     <term>Description</term>
    /// </listheader>
    /// <item>
    ///     <term>4/16/2020</term>
    ///     <term>Christian Azula</term>
    ///     <term>RM-80</term>
    ///     <term>Initial Implementation</term>
    /// </item>
    /// </list>
    /// </remarks>
    public class CustomOperationFilter : IOperationFilter
    {
        #region Public Methods

        /// <summary>
        /// Removes the API Version as a parameter from the Swagger document
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var versionParameter = operation.Parameters.Single(p => p.Name == "version");
            operation.Parameters.Remove(versionParameter);
        }

        #endregion
    }
}
