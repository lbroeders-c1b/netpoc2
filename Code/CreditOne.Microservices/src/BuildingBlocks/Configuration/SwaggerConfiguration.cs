using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using CreditOne.Microservices.BuildingBlocks.Common.Configuration.Swagger;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CreditOne.Microservices.BuildingBlocks.Common.Configuration
{
    /// <summary>
    /// Implements the SwaggerConfiguration class
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///     <term>Date</term>
    ///		<term>Who</term>
    ///		<term>BR/WO</term>
    ///		<description>Description</description>
    /// </listheader>
	/// <item>
	///		<term>04-24-2019</term>
	///		<term>Armando Soto</term>
	///		<term>RM-47</term>
	///		<description>Initial implementation</description>
	/// </item>
    /// </list>
    /// </remarks> 	
    public class SwaggerConfiguration
    {
        #region Public Properties

        public string Version { get; set; }

        public string UIEndpoint { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the path for the work case api xml file
        /// </summary>
        /// <returns></returns>
        public static string GetXmlCommentsPath(string xmlFile)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFile);
        }

        /// <summary>
        /// Predicate to set customs decorator in order to read api version information
        /// </summary>
        /// <returns>return true if found api version decorator</returns>
        public static Func<string, ApiDescription, bool> MapApiVersionAttributes()
        {
            return (version, desc) =>
            {
                var getControllerInfoSuccess = CustomReflectionUtils.TryGetControllerTypeInfo(desc, out TypeInfo controllerInfo);

                var versions = controllerInfo
                    .GetCustomAttributes<ApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions);

                var getMethodInfoSuccess = desc.TryGetMethodInfo(out MethodInfo methodInfo);

                var maps = methodInfo
                    .GetCustomAttributes<MapToApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions)
                    .ToArray();

                return versions.Any(v => $"v{v.ToString()}" == version) && (!maps.Any() || maps.Any(v => $"v{v.ToString()}" == version));
            };
        }

        /// <summary>
        /// Sets bearer token configuration
        /// </summary>
        /// <param name="options"></param>
        public static void SetBearerToken(SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("Bearer", new ApiKeyScheme()
            {
                Description = "Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = "header",
                Type = "apiKey"
            });

            options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
            {
                { "Bearer", new string[] { } }
            });
        }

        #endregion
    }
}
