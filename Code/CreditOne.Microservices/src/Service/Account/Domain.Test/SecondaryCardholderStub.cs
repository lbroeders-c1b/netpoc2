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
    ///     <term>11/21/2019</term>
    ///     <term>Armando Soto</term>
    ///     <term>RM-47</term>
    ///     <description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks> 
    public class SecondaryCardholderStub
    {
        #region Private Members

        private IList<SecondaryCardholder> _secondaryCardholders;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public SecondaryCardholderStub()
        {
            _secondaryCardholders = new List<SecondaryCardholder>()
            {
                new SecondaryCardholder()
                {
                    Id = 1,
                    CreditAccountId = 1180,
                    FirstName = "John",
                    LastName = "Doe"
                },

                new SecondaryCardholder()
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
        /// Mocks a Secondary Cardholder.
        /// </summary>
        /// <returns> Valid Secondary Cardholder</returns>
        public SecondaryCardholder GetValidSecondaryCardholder()
        {
            return _secondaryCardholders.First();
        }

        /// <summary>
        /// Mocks a empty Secondary Cardholder.
        /// </summary>
        /// <returns> Emplty Secondary Cardholder</returns>
        public SecondaryCardholder GetEmptySecondaryCardholder()
        {
            return null;
        }

        /// <summary>
        /// Retrieves the list of Secondary Cardholder
        /// </summary>
        /// <returns></returns>
        public IList<SecondaryCardholder> GetSecondaryCardholders()
        {
            return _secondaryCardholders;
        }

        #endregion
    }
}
