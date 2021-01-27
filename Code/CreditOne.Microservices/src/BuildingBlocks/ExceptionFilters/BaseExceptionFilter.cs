using System;
using System.Net;

using CreditOne.Microservices.BuildingBlocks.ExceptionFilters.KnownExceptions;
using CreditOne.Microservices.BuildingBlocks.HttpExtensions;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CreditOne.Microservices.BuildingBlocks.ExceptionFilters
{
    /// <summary>
    /// Represents the base exception filter class
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
    ///     <term>4/3/2020</term>
    ///     <term>Christian Azula</term>
    ///     <term>RM-80</term>
    ///     <description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks>
    public class BaseExceptionFilter : IExceptionFilter
    {
        #region Private Members

        private const string UnexpectedErrorMessage =
            "Unexpected error occurred: {0} at {1} contact Helpdesk. Exception message: [{2}].";

        private readonly ILogger _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">Logger</param>
        public BaseExceptionFilter(ILogger<BaseExceptionFilter> logger)
        {
            _logger = logger ?? throw new NullReferenceException(nameof(ILogger<BaseExceptionFilter>));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks if the exception is a custom exception
        /// </summary>
        /// <param name="context">Exception context</param>
        /// <returns>True if the exception is custom</returns>
        public virtual bool IsCustomException(ExceptionContext context)
        {
            return context.Exception is ICustomException;
        }

        /// <summary>
        /// Handles custom exceptions
        /// </summary>
        /// <param name="context">exception context</param>
        /// <returns>True if the exception is custom</returns>
        public virtual bool TryHandleCustomException(ExceptionContext context)
        {
            return IsCustomException(context);
        }

        /// <summary>
        /// Handles known exceptions
        /// </summary>
        /// <param name="context">exception context</param>
        /// <returns>True if it is a known exception and 
        /// update the context.Result and
        /// context.HttpContext.Reponse.StatusCode</returns>
        public virtual bool TryHandleKnownExceptionResults(ExceptionContext context)
        {
            bool isKnownExeptionHandled = false;

            if (context.Exception.GetType() == typeof(BadRequestException))
            {
                context.Result = new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(context.Exception.Message);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                isKnownExeptionHandled = true;
            }
            else if (context.Exception.GetType() == typeof(NotFoundException))
            {
                context.Result = new Microsoft.AspNetCore.Mvc.NotFoundObjectResult(context.Exception.Message);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                isKnownExeptionHandled = true;
            }

            return isKnownExeptionHandled;
        }

        /// <summary>
        /// Called after an action has thrown an Exception
        /// </summary>
        /// <param name="context">Exception context</param>
        public void OnException(ExceptionContext context)
        {
            var formatedRequest = HttpRequestExtension.FormatRequest(context.HttpContext.Request);

            _logger.LogError(context.Exception, formatedRequest);

            if (!TryHandleCustomException(context) &&
                !TryHandleKnownExceptionResults(context))
            {
                var message = string.Format(UnexpectedErrorMessage, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), context.Exception.Message);

                context.Result = new InternalServerErrorObjectResult(message);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            context.ExceptionHandled = true;
        }

        #endregion
    }
}
