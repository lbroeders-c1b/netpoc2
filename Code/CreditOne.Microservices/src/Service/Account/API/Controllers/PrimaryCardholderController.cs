using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using AutoMapper;

using CreditOne.Microservices.Account.IDataProvider;
using CreditOne.Microservices.Account.Models;
using CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Factory;
using CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Filter;

using Microsoft.AspNetCore.Mvc;

namespace CreditOne.Microservices.Account.API.Controllers
{
    /// <summary>
    /// Implements the primary cardholder controller
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
    ///		<term>05-30-2019</term>
    ///		<term>Christian Azula</term>
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
    [Route("api/v{version:apiVersion}/creditaccounts/{creditAccountId}/primary-cardholder")]
    [ApiController]
    public class PrimaryCardholderController : ControllerBase
    {
        #region Private Members

        private readonly IPrimaryCardholderDataProvider _primaryCardholderDataProvider;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>      
        public PrimaryCardholderController(IMapper mapper,
                                           IPrimaryCardholderDataProvider primaryCardholderDataProvider)
        {
            _mapper = mapper;
            _primaryCardholderDataProvider = primaryCardholderDataProvider;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the primary card holder for a given credit account ID
        /// </summary>
        /// <remarks>
        /// Sample Request:
        ///        
        ///     GET /api/v1/creditaccounts/2372706/primary-cardholder
        ///     
        ///     Credit Account Id: 2372706
        ///
        /// </remarks>
        /// <param name="creditAccountId">Credit Account ID</param>
        /// <returns>Primary Card Holder Information</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PrimaryCardholderModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [ActionParameter("creditAccountId", EnumValidation.Id)]
        public async Task<IActionResult> GetByCreditAccountId(decimal creditAccountId)
        {
            var primaryCardholder = _mapper.Map<PrimaryCardholderModel>((await _primaryCardholderDataProvider.GetByCreditAccountId(creditAccountId)).FirstOrDefault());

            if (primaryCardholder == null)
            {
                return Ok();
            }

            return Ok(primaryCardholder);
        }

        #endregion
    }
}
