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
    /// Implements the secondary cardholder controller
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
    ///		<term>10-03-2019</term>
    ///		<term>arsoto</term>
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
    [Route("api/v{version:apiVersion}/creditaccounts/{creditAccountId}/secondary-cardholder")]
    [ApiController]
    public class SecondaryCardholderController : ControllerBase
    {
        #region Private Members

        private readonly IMapper _mapper;
        private readonly ISecondaryCardholderDataProvider _secondaryCardholderDataProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>      
        public SecondaryCardholderController(IMapper mapper,
                                             ISecondaryCardholderDataProvider secondaryCardholderDataProvider)
        {
            _mapper = mapper;
            _secondaryCardholderDataProvider = secondaryCardholderDataProvider;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the secondary card holder for a given credit account ID
        /// </summary>
        /// <remarks>
        /// Sample Request:
        ///        
        ///     GET /api/v1/creditaccounts/2372706/secondary-cardholder
        ///     
        ///     Credit Account Id: 2372706
        ///
        /// </remarks>
        /// <param name="creditAccountId">Credit Account ID</param>
        /// <returns>Secondary Holder Information</returns>  
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(SecondaryCardholderModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [ActionParameter("creditAccountId", EnumValidation.Id)]
        public async Task<IActionResult> GetByCreditAccountId(decimal creditAccountId)
        {
            var secondaryCardholder = _mapper.Map<SecondaryCardholderModel>((await _secondaryCardholderDataProvider.GetByCreditAccountId(creditAccountId)).FirstOrDefault());

            if (secondaryCardholder == null)
            {
                return Ok();
            }

            return Ok(secondaryCardholder);
        }

        #endregion
    }
}
