using System;
using System.Collections.Generic;
using System.Text;

namespace CreditOne.Microservices.Account.Domain.Test
{
    /// <summary>
    /// Implements the stub for card entity used on unit tests
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///	    <term>Date</term>
    ///	    <term>Who</term>
    ///	    <term>BR/WO</term>
    ///	    <description>Description</description>
    /// </listheader>
    /// <item>
    ///	    <term>9/3/2019</term>
    ///	    <term>Federico Bendayan</term>
    ///	    <term>RM-47</term>
    ///	    <description>Initial implementation</description>  
    ///	</item>
    /// </list>
    /// </remarks> 
    public class CardStub
    {
        #region Private Members

        private List<Card> list;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public CardStub()
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Rerieves the list of valid cards
        /// </summary>
        /// <returns></returns>
        public List<Card> GetValidCards()
        {
            list = new List<Card>()
            {
                new Card()
                {
                    Id = 1,
                }
            };

            return list;
        }

        #endregion
    }
}
