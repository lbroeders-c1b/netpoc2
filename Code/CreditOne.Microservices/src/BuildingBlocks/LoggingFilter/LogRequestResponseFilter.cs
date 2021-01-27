using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace CreditOne.Microservices.BuildingBlocks.LoggingFilter
{
    /// <summary>
    /// Filter to log every request and respoonse made to the api depending on configuration.
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///  <term>Date</term>
    ///  <term>Who</term>
    ///  <term>BR/WO</term>
    ///  <description>Description</description>
    /// </listheader>
    /// <item>
    ///  <term>05-12-2020</term>
    ///  <term>Daniel Lobaton</term>
    ///  <term>WO376694</term>
    ///  <description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks>
    public class LogRequestResponseFilter : IAsyncActionFilter
    {
        #region Private Members

        private readonly ILogger _logger;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for the class LogRequestResponseFilter.
        /// </summary>      
        public LogRequestResponseFilter(ILogger<LogRequestResponseFilter> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(ILogger<LogRequestResponseFilter>));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Called asynchronously before the action, after model binding is complete.
        /// </summary>
        public async Task OnActionExecutionAsync(ActionExecutingContext actionContext, ActionExecutionDelegate next)
        {
            var controllerAction = actionContext.ActionDescriptor as ControllerActionDescriptor;

            var request = new
            {
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff zzz"),
                Endpoint = $"{controllerAction.ControllerName}/{controllerAction.ActionName}",
                Route = $"{controllerAction.AttributeRouteInfo.Template}",
                Payload = actionContext.ActionArguments
            };

            var message = JsonConvert.SerializeObject(request);
            _logger.LogInformation(message);

            AddStopwatchToRequest(actionContext);

            var resultContext = await next();

            try
            {
                var controllerName = (resultContext.ActionDescriptor as ControllerActionDescriptor).ControllerName;

                var response = new
                {
                    Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff zzz"),
                    Endpoint = $"{controllerName}/{(resultContext.ActionDescriptor as ControllerActionDescriptor).ActionName}",
                    HttpStatusCode = resultContext.Exception != null ? HttpStatusCode.InternalServerError : GetStatusCode(resultContext.Result),
                    ElapsedTimeMilliseconds = GetElapsedTime(resultContext)
                };

                var messageResponse = JsonConvert.SerializeObject(response);
                _logger.LogInformation(messageResponse);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes a new System.Diagnostics.Stopwatch instance, sets the elapsed time
        /// property to zero, and starts measuring elapsed time.
        /// </summary>
        private void AddStopwatchToRequest(ActionExecutingContext actionContext)
        {
            var stopwatch = Stopwatch.StartNew();
            actionContext.HttpContext.Items.Add(this.GetType().FullName, stopwatch);
        }

        /// <summary>
        /// Stops measuring elapsed time for an interval and retrieves the elapsed time.
        /// </summary>
        private long GetElapsedTime(ActionExecutedContext actionExecutedContext)
        {
            long result = 0;
            var key = GetType().FullName;
            if (!actionExecutedContext.HttpContext.Items.ContainsKey(key))
            {
                return 0;
            }

            var stopwatch = actionExecutedContext.HttpContext.Items[key] as Stopwatch;
            if (stopwatch != null)
            {
                stopwatch.Stop();
                result = stopwatch.ElapsedMilliseconds;
            }

            return result;
        }

        /// <summary>
        /// Gets the status code from an object that implements the IActionResult
        /// </summary>
        /// <param name="actionResult">The object that implements IActionResult</param>
        /// <returns>An HttpStatusCode. Returns<c>HttpStatusCode.InternalServerError</c> when IActionResult object cannot find its type</returns>
        private HttpStatusCode GetStatusCode(IActionResult actionResult)
        {
            HttpStatusCode httpStatusCode;

            if (actionResult is StatusCodeResult)
            {
                httpStatusCode = (HttpStatusCode)((StatusCodeResult)actionResult).StatusCode;
            }
            else if (actionResult is ObjectResult)
            {
                if (((ObjectResult)actionResult).StatusCode.HasValue)
                {
                    httpStatusCode = (HttpStatusCode)((ObjectResult)actionResult).StatusCode.Value;
                }
                else
                {
                    throw new ArgumentNullException("StatusCode");
                }
            }
            else
            {
                throw new InvalidCastException("IActionResult");
            }

            return httpStatusCode;
        }

        #endregion
    }
}