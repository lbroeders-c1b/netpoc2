using AutoMapper;

using CreditOne.Microservices.Account.Models;

namespace CreditOne.Microservices.Account.Service.Model
{
    /// <summary>
    /// Implements the customer profile class
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
	///		<term>10/16/2019</term>
	///		<term>Armando Soto</term>
	///		<term>RM-47</term>
	///		<description>Initial implementation</description>
	/// </item>
    /// </list>
    /// </remarks> 	
    public class CustomerProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerProfile()
        {
            CreateMap<CustomerModel, Domain.Customer>()
                .ForMember(x => x.SocialSecurityNumber, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
