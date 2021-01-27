using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using AutoMapper;

using CreditOne.Microservices.Account.IDataProvider;
using CreditOne.Microservices.Account.Models;
using CreditOne.Microservices.Account.Models.Response;
using CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Factory;
using CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Filter;

using Microsoft.AspNetCore.Mvc;

namespace CreditOne.Microservices.Account.API.Controllers
{
    /// <summary>
    /// Implements the email controller
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
    ///		<term>09/01/2020</term>
    ///		<term>Daniel Lobaton</term>
    ///		<term>RM-79</term>
    ///		<description>Initial implementation</description>  
    /// </item>
    /// </list>
    /// </remarks> 
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/creditaccounts/{creditAccountId}")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        #region Private Members

        private readonly IEmailDataProvider _emailDataProvider;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>      
        public EmailController(IMapper mapper, IEmailDataProvider emailDataProvider)
        {
            _mapper = mapper;
            _emailDataProvider = emailDataProvider;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the emails for a given credit account ID
        /// </summary>
        /// <remarks>
        /// Sample Request:
        ///        
        ///     GET /api/v1/creditaccounts/2372706/emails
        ///     
        ///     Credit Account Id: 2372706
        ///
        /// </remarks>
        /// <param name="creditAccountId">Credit Account ID</param>
        /// <returns>Email Information</returns>
        [HttpGet]
        [Route("emails")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<CardModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [ActionParameter("creditAccountId", EnumValidation.Id)]
        public async Task<IActionResult> GetByCreditAccountId(decimal creditAccountId)
        {
            var emails = _mapper.Map<List<CreditAccountEmailResponse>>(await _emailDataProvider.GetByCreditAccountId(creditAccountId));

            return Ok(emails);
        }

        #endregion
    }
}