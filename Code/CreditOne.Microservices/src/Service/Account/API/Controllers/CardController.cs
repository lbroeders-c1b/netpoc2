using System;
using System.Collections.Generic;
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
    /// Implements the card controller
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
    ///		<term>5/27/2019</term>
    ///		<term>Armando Soto</term>
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
    [Route("api/v{version:apiVersion}/creditaccounts/{creditAccountId}")]
    [ApiController]
    public class CardController : ControllerBase
    {
        #region Private Members

        private readonly ICardDataProvider _cardDataProvider;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>      
        public CardController(IMapper mapper,
                              ICardDataProvider cardDataProvider)
        {
            _mapper = mapper;
            _cardDataProvider = cardDataProvider;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves a card for a given credit account ID
        /// </summary>
        /// <remarks>
        /// Sample Request:
        ///        
        ///     GET /api/v1/creditaccounts/2372706/cards
        ///     
        ///     Credit Account Id: 2372706
        ///
        /// </remarks>
        /// <param name="creditAccountId">Credit Account ID</param>
        /// <returns>Card Information</returns>
        [HttpGet]
        [Route("cards")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<CardModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [ActionParameter("creditAccountId", EnumValidation.Id)]
        public async Task<IActionResult> GetByCreditAccountId(decimal creditAccountId)
        {
            var cards = _mapper.Map<List<CardModel>>(await _cardDataProvider.GetByCreditAccountId(creditAccountId));

            return Ok(cards);
        }

        #endregion
    }
}