using System;
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
    /// Implements the customer controller
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
    ///     <term>03-21-2019</term>
    ///     <term>Federico Bendayan</term>
    ///     <term>RM-47</term>
    ///     <description>Initial implementation</description>
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
    [Route("api/v{version:apiVersion}/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        #region Private Members

        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerDataProvider _customerDataProvider;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>      
        public CustomerController(IMapper mapper,
                                  ICustomerRepository customerRepository,
                                  ICustomerDataProvider customerDataProvider)
        {
            _mapper = mapper;
            _customerRepository = customerRepository;
            _customerDataProvider = customerDataProvider;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves a customer for a given Ssn
        /// </summary>
        /// <remarks>
        /// Sample Request:
        ///        
        ///     POST /api/v1/creditaccounts
        ///     
        ///     Request Body = xxxxxx779 (SSN)
        ///     
        /// </remarks>
        /// <param name="ssn">Social security number</param>
        /// <returns>Customer information</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(CustomerModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetBySSN([FromBody] string ssn)
        {
            var customers = _mapper.Map<CustomerModel>((await _customerDataProvider.GetBySSN(ssn)).FirstOrDefault());

            if (customers == null)
            {
                throw new NotFoundException($"The Customer for the given SSN [{ssn}] does not exist!");
            }

            return Ok(customers);
        }

        /// GET api/v1/customers/2171778
        /// <summary>
        /// Retrieves a customer for a given ID
        /// </summary>
        /// <remarks>
        /// Sample Request:
        ///        
        ///     GET /api/v1/customers/2728967
        ///     
        ///     Customer Id: 2728967
        ///
        /// </remarks>
        ///  <param name="id">Customer ID</param>
        /// <returns>Customer information</returns>  
        [HttpGet]
        [Route("{id:decimal}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CustomerModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [ActionParameter("id", EnumValidation.Id)]
        public async Task<IActionResult> GetById(decimal id)
        {
            var customer = _mapper.Map<CustomerModel>(await _customerRepository.GetById(id));

            if (customer == null)
            {
                throw new NotFoundException($"The Customer ID [{id}] does not exist!");
            }

            return Ok(customer);
        }

        #endregion
    }
}