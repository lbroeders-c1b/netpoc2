using System.Collections.Generic;

namespace CreditOne.Microservices.Account.Domain.Test
{
    /// <summary>
    /// Implements the stub for address entity used on unit tests
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
    ///		<term>5/29/2019</term>
    ///		<term>Daniel Lobaton</term>
    ///		<term>RM-47</term>
    ///		<description>Initial implementation</description>  
    ///	</item>
    /// </list>
    /// </remarks> 
    public class AddressStub
    {
        #region Private Members

        private readonly List<Address> list;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public AddressStub()
        {
            list = new List<Address>()
            {
                new Address()
                {
                    Id = 1,
                    AddressLineOne = "Address line 1",
                    AddressLineTwo = "Address line 2",
                    City = "City",
                    State = "NV",
                    Zip = "113456"
                }
            };
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the addresses
        /// </summary>
        /// <returns></returns>
        public List<Address> GetAll()
        {
            return list;
        }

        #endregion
    }
}
