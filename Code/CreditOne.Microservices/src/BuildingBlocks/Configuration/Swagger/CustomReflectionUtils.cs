using System.Reflection;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;

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
    public static class CustomReflectionUtils
    {
        #region Public Methods

        /// <summary>
        /// Gets controller type info
        /// </summary>
        /// <param name="apiDescription"></param>
        /// <param name="controllerTypeInfo"></param>
        /// <returns></returns>
        public static bool TryGetControllerTypeInfo(this ApiDescription apiDescription, out TypeInfo controllerTypeInfo)
        {
            var controllerActionDescriptor = apiDescription.ActionDescriptor as ControllerActionDescriptor;

            controllerTypeInfo = controllerActionDescriptor?.ControllerTypeInfo;

            return (controllerTypeInfo != null);
        }

        #endregion
    }
}
