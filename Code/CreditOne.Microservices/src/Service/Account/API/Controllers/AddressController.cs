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
    /// Implements the address controller
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
    ///		<term>05-27-2019</term>
    ///		<term>Daniel Lobaton</term>
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
    [Route("api/v{version:apiVersion}/creditaccounts/{creditAccountId}/addresses")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        #region Private Members

        private readonly IAddressDataProvider _addressDataProvider;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>      
        public AddressController(IMapper mapper,
                                 IAddressDataProvider addressDataProvider)
        {
            _mapper = mapper;
            _addressDataProvider = addressDataProvider;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves a address for a given credit account ID
        /// </summary>
        /// <remarks>
        /// Sample Request:
        ///        
        ///     GET /api/v1/creditaccounts/2372706/addresses
        ///     
        ///     Credit Account Id: 2372706
        ///
        /// </remarks>
        /// <param name="creditAccountId">Credit Account ID</param>
        /// <returns>Address Information</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<AddressModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [ActionParameter("creditAccountId", EnumValidation.Id)]
        public async Task<IActionResult> GetByCreditAccountId(decimal creditAccountId)
        {
            var addresses = _mapper.Map<List<AddressModel>>(await _addressDataProvider.GetByCreditAccountId(creditAccountId));

            return Ok(addresses);
        }

        #endregion
    }
}