using System;
using System.Net;
using System.Reflection;

using Microsoft.AspNetCore.Mvc;

namespace CreditOne.Microservices.BuildingBlocks.Controllers.Configuration
{
    /// Implements health check controller class
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
    ///     <term>7/14/2020</term>
    ///     <term>Mauro Allende</term>
    ///     <term>RM-79</term>
    ///     <description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks>
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/health")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        /// <summary>
        /// Checks if the microservice is running
        /// </summary>
        /// <remarks>
        /// Sample Request:   
        /// 
        ///     GET /api/v1/health
        ///          
        /// </remarks>        
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public ActionResult<string> Check()
        {
            return Ok($"{Assembly.GetEntryAssembly().GetName().Name} is up and running as is at {DateTime.Now.ToString()}. Server Name: {Environment.MachineName}.");
        }
    }
}
