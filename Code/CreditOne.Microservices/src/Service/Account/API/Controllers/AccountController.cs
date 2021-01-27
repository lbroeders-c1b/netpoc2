using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using CreditOne.Microservices.Account.Models;
using CreditOne.Microservices.Account.Service;
using CreditOne.Microservices.BuildingBlocks.ExceptionFilters.KnownExceptions;

using Microsoft.AspNetCore.Mvc;

namespace CreditOne.Microservices.Account.API.Controllers
{
    /// <summary>
    /// Implements the account controller
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
    ///		<term>10/3/2019</term>
    ///		<term>Luis Petitjean</term>
    ///		<term>RM-47</term>
    ///		<description>Initial implementation</description>
    /// </item>
    /// <item>
    ///		<term>8/21/2020</term>
    ///		<term>Mauro Allende</term>
    ///		<term>RM-79</term>
    ///		<description>Refactor action methods</description>  
    /// </item>
    /// </list>
    /// </remarks> 	
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Private Members

        private readonly IAccountService _accountService;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Searches the customer demographics by a given criteria.
        /// </summary>
        /// <remarks>
        /// Sample Request:
        ///        
        ///     POST /api/v1/accounts
        ///     
        ///     {
        ///         "firstName": "MARROQUIN",
        ///         "lastName": "HENLEY",
        ///         "cardPrefix": "1213"
        ///     }
        ///
        /// </remarks>
        /// <param name="accountSearchModel">Search criteria parameters</param>
        /// <returns>List of Accounts</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<AccountModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(AccountSearchModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchBy([FromBody] AccountSearchModel accountSearchModel)
        {
            var result = await _accountService.SearchBy(accountSearchModel);

            if (result.accountModels == null)
            {
                throw new NotFoundException($"Search by [{result.inferedSearch}] was infered from the request and no result was found");
            }

            return Ok(result.accountModels);
        }

        #endregion        
    }
}