using System.Collections.Generic;
using System.Linq;

namespace CreditOne.Microservices.Account.Domain.Test
{
    /// <summary>
    /// Implements test data for unit testing the api
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
    ///     <term>5/31/2019</term>
    ///     <term>Christian Azula</term>
    ///     <term>RM-47</term>
    ///     <description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks> 
    public class PrimaryCardholderStub
    {
        #region Private Members

        private IList<PrimaryCardholder> _primaryCardholders;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public PrimaryCardholderStub()
        {
            _primaryCardholders = new List<PrimaryCardholder>()
            {
                new PrimaryCardholder()
                {
                    Id = 1,
                    CreditAccountId = 1180,
                    FirstName = "John",
                    LastName = "Doe"
                },

                new PrimaryCardholder()
                {
                    Id = 2,
                    CreditAccountId = 100,
                    FirstName = "First",
                    LastName = "Last"
                }
            };
        }

        #endregion

        #region Public Memebers

        /// <summary>
        /// Mocks a Primary Cardholder.
        /// </summary>
        /// <returns> Valid Primary Cardholder</returns>
        public PrimaryCardholder GetValidPrimaryCardholder()
        {
            return _primaryCardholders.First();
        }

        /// <summary>
        /// Mocks a empty Primary Cardholder.
        /// </summary>
        /// <returns> Emplty Primary Cardholder</returns>
        public PrimaryCardholder GetEmptyPrimaryCardholder()
        {
            return null;
        }

        /// <summary>
        /// Retrieves the list of Primary Cardholder
        /// </summary>
        /// <returns></returns>
        public IList<PrimaryCardholder> GetPrimaryCardholders()
        {
            return _primaryCardholders;
        }

        #endregion
    }
}
