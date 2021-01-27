using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using AutoMapper;

using CreditOne.Microservices.Account.Domain;
using CreditOne.Microservices.Account.IDataProvider;
using CreditOne.Microservices.Account.Models;
using CreditOne.Microservices.BuildingBlocks.ExceptionFilters.KnownExceptions;
using CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Factory;
using CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Filter;

using Microsoft.AspNetCore.Mvc;

namespace CreditOne.Microservices.Account.API.Controllers
{
    /// <summary>
    /// Implements the credit account controller
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
    /// </list>
    /// </remarks> 
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/creditaccounts")]
    [ApiController]
    public class CreditAccountController : ControllerBase
    {
        #region Private Constants

        private const string ErrorMessageBadNumberAccountFound = "More than one account found";

        #endregion

        #region Private Members

        private readonly ICreditAccountRepository _creditAccountRepository;
        private readonly ICreditAccountDataProvider _creditAccountDataProvider;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>      
        public CreditAccountController(IMapper mapper,
                                       ICreditAccountDataProvider creditAccountDataProvider,
                                       ICreditAccountRepository creditAccountRepository)
        {
            _mapper = mapper;
            _creditAccountDataProvider = creditAccountDataProvider;
            _creditAccountRepository = creditAccountRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves a credit account for a given ID
        /// </summary>
        /// <remarks>
        /// Sample Request:
        ///        
        ///     GET /api/v1/creditaccounts/2372706
        ///     
        ///     Credit Account Id: 2372706
        ///
        /// </remarks>
        /// <param name="id">Credit Account ID</param>
        /// <returns>Credit Account Information</returns>
        [HttpGet]
        [Route("{id:decimal}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CreditAccountModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [ActionParameter("id", EnumValidation.Id)]
        public async Task<IActionResult> GetById(decimal id)
        {
            var creditAccounts = _mapper.Map<List<CreditAccountModel>>(await _creditAccountRepository.GetById(id));

            if (creditAccounts == null || creditAccounts.Count <= 0)
            {
                throw new NotFoundException($"The Credit Account ID [{id}] does not exist!");
            }

            if (creditAccounts.Count > 1)
            {
                throw new Exception(ErrorMessageBadNumberAccountFound);
            }

            return Ok(creditAccounts.FirstOrDefault());
        }

        /// <summary>
        /// Retrieves a credit account for a given customer ID
        /// </summary>
        /// <remarks>
        /// Sample Request:
        ///        
        ///     GET /api/v1/creditaccounts?customerId=2728967
        ///     
        ///     Customer Id: 2728967
        ///
        /// </remarks>
        /// <param name="customerId">Customer ID</param>
        /// <returns>Credit Account Information</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<CreditAccountModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [ActionParameter("customerId", EnumValidation.Id)]
        public async Task<IActionResult> GetByCustomerId(decimal customerId)
        {
            var creditAccounts = _mapper.Map<List<CreditAccountModel>>(await _creditAccountDataProvider.GetByCustomerId(customerId));

            if (creditAccounts == null || creditAccounts.Count <= 0)
            {
                throw new NotFoundException($"The Credit Account for Customer ID [{customerId}] does not exist!");
            }

            return Ok(creditAccounts);
        }

        /// <summary>
        /// Retrieves a credit account for a given card number
        /// </summary>
        /// <remarks>
        /// Sample Request:
        ///        
        ///     POST /api/v1/creditaccounts
        ///     
        ///     Request Body = xxxxxxxxxxxx9159 (Card Number)
        ///     
        /// </remarks>
        ///  <param name="cardNumber">The card number for the credit account searching</param>
        /// <returns>Credit Account Information</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CreditAccountModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByCardNumber([FromBody] string cardNumber)
        {
            var creditAccounts = _mapper.Map<List<CreditAccountModel>>(await _creditAccountDataProvider.GetByCardNumber(cardNumber));

            if (creditAccounts == null || creditAccounts.Count <= 0)
            {
                throw new NotFoundException($"The Credit Account for the given card number does not exist!");
            }

            if (creditAccounts.Count > 1)
            {
                throw new Exception(ErrorMessageBadNumberAccountFound);
            }

            return Ok(creditAccounts.FirstOrDefault());
        }

        #endregion
    }
}
