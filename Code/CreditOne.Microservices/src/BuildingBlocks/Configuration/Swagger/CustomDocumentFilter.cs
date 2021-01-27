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
    public class CustomDocumentFilter : IDocumentFilter
    {
        #region Public Methods

        /// <summary>
        /// Change the path from action/controller "map to api" decorator
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Paths = swaggerDoc.Paths
                .ToDictionary(
                    path => path.Key.Replace("v{version}", swaggerDoc.Info.Version),
                    path => path.Value
                );
        }

        #endregion
    }
}
