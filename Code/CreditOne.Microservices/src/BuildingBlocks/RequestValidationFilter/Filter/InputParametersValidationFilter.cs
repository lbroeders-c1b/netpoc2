using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Factory;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Filter
{
    /// <summary>
    /// Base class to get parameters in order to be validated
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
    public class InputParametersValidationFilter : IAsyncActionFilter
    {
        private readonly ILogger _logger;
        private readonly IValidatorFactory _factory;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="logger">Logger interface</param>
        /// <param name="validatorFactory">Validator factory interface</param>
        public InputParametersValidationFilter(ILogger<InputParametersValidationFilter> logger,
                                               IValidatorFactory validatorFactory)
        {
            this._logger = logger ?? throw new NullReferenceException(nameof(ILogger<InputParametersValidationFilter>));

            this._factory = validatorFactory ?? throw new NullReferenceException(nameof(IValidatorFactory));
        }

        /// <summary>
        /// Implementation of OnActionExecutionAsync event 
        /// </summary>
        /// <param name="context">context for action filters</param>
        /// <param name="next">a delegate that asynchronously returns an Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext</param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var errorResults = new List<string>();

            var action = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo
                as MethodInfo;

            await Task.Run(() => this.ValidateSimpleParameters(action, context, ref errorResults));

            await Task.Run(() => this.ValidateModelParameters(action, context, ref errorResults));

            if (errorResults.Count > 0)
            {
                var message = $"Validation error(s):\r\n{string.Join("\r\n", errorResults)}";

                this._logger.LogError(message);

                context.Result = new BadRequestObjectResult(message);

                return;
            }

            await next();
        }

        /// <summary>
        /// Validates action parameters
        /// </summary>
        /// <param name="action">action parameter</param>
        /// <param name="context">context for action filters</param>
        /// <param name="errorResults">list of errors</param>
        /// <returns></returns>
        private void ValidateSimpleParameters(MethodInfo action,
            ActionExecutingContext context, ref List<string> errorResults)
        {
            var actionParametersToValidate = action.GetCustomAttributes<ActionParameter>();

            foreach (var parameter in actionParametersToValidate)
            {
                dynamic actionArgument = null;

                if (context.ActionArguments.ContainsKey(parameter.ParamName) && context.ActionArguments[parameter.ParamName] != null)
                {
                    actionArgument = context.ActionArguments[parameter.ParamName];

                    var validator = this._factory.GetValidator(parameter.Validation);

                    if (validator != null)
                    {
                        validator.IsValid(actionArgument, parameter, ref errorResults);
                    }
                }
                else if (parameter.Required)
                {
                    errorResults.Add($"The parameter [{parameter.ParamName}] is required.");

                    continue;
                }
            }
        }

        /// <summary>
        /// Validates model parameters from an action
        /// </summary>
        /// <param name="action">action parameter</param>
        /// <param name="context">context for action filters</param>        
        /// <param name="errorResults">list of errors</param>
        private void ValidateModelParameters(MethodInfo action,
            ActionExecutingContext context, ref List<string> errorResults)
        {
            var actionParameters = action.GetParameters()
                .Where(x => context.ActionArguments.TryGetValue(x.Name, out object parameterValue) &&
                parameterValue != null &&
                parameterValue.GetType().GetProperties().Where(w => w.GetCustomAttribute<ActionParameter>() != null).Any());

            foreach (var parameter in actionParameters)
            {
                object actionArgument = context.ActionArguments[parameter.Name];

                var properties = actionArgument.GetType().GetProperties()
                    .Where(x => x.GetCustomAttribute<ActionParameter>() != null);

                foreach (var property in properties)
                {
                    var decorator = property.GetCustomAttribute<ActionParameter>();

                    dynamic propertyValue = property.GetValue(actionArgument);

                    if (propertyValue != null)
                    {
                        Type propertyType = propertyValue.GetType();

                        if (propertyType == typeof(string) && string.IsNullOrEmpty(propertyValue) && decorator.Required)
                        {
                            errorResults.Add($"The paramater [{actionArgument.GetType().Name}.{property.Name}] is required.");

                            continue;
                        }

                        var validator = this._factory.GetValidator(decorator.Validation);

                        if (validator != null)
                        {
                            validator.IsValid(propertyValue, decorator, ref errorResults);
                        }
                    }
                    else if (decorator.Required)
                    {
                        errorResults.Add($"The parameter [{decorator.ParamName}] is required.");

                        continue;
                    }

                    object valueModel = property.GetValue(actionArgument);

                    if (valueModel != null)
                    {
                        var modelProperties = property.GetValue(actionArgument).GetType().GetProperties()
                                                      .Where(w => w.GetCustomAttribute<ActionParameter>() != null);

                        foreach (var modelProperty in modelProperties)
                        {
                            var modelDecorator = modelProperty.GetCustomAttribute<ActionParameter>();

                            dynamic modelPropertyValue = modelProperty.GetValue(valueModel);

                            if (modelPropertyValue != null)
                            {
                                Type modelPropertyType = modelPropertyValue.GetType();

                                if (modelPropertyType == typeof(string) && string.IsNullOrEmpty(modelPropertyValue) && decorator.Required)
                                {
                                    errorResults.Add($"The paramater [{valueModel.GetType().Name}.{modelProperty.Name}] is required.");

                                    continue;
                                }

                                var validator = this._factory.GetValidator(modelDecorator.Validation);

                                if (validator != null)
                                {
                                    validator.IsValid(modelPropertyValue, modelDecorator, ref errorResults);
                                }
                            }
                            else if (modelDecorator.Required)
                            {
                                errorResults.Add($"The parameter [{valueModel.GetType().Name}.{modelProperty.Name}] is required.");

                                continue;
                            }
                        }
                    }
                }
            }
        }
    }
}
