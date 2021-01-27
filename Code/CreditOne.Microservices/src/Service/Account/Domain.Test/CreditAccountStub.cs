using System.Collections.Generic;

namespace CreditOne.Microservices.Account.Domain.Test
{
    /// <summary>
    /// Implements the stub for credit account entity used on unit tests
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
    ///	</item>
    /// </list>
    /// </remarks> 
    public class CreditAccountStub
    {
        #region Private Members

        private readonly List<CreditAccount> list;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public CreditAccountStub()
        {
            list = new List<CreditAccount>()
            {
                new CreditAccount()
                {
                    Id = 1,
                    CustomerId = 1,
                    Card = new Card()
                    {
                        Id = 1,
                        IsCurrent = true,
                        FullCardNumber = "4234567891234567"
                    }
                },

                new CreditAccount()
                {
                    Id = 2,
                    CustomerId = 1,
                    Card = new Card()
                    {
                        Id = 1,
                        IsCurrent = false,
                        FullCardNumber = "4234567891234568"
                    }
                }
            };
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the list of all Credit Account Stubed
        /// </summary>
        /// <returns></returns>
        public List<CreditAccount> GetAll()
        {
            return list;
        }

        /// <summary>
        /// Retrieves a list with one element wich have its current active card
        /// </summary>
        /// <returns></returns>
        public List<CreditAccount> GetCreditAccountWithAnActiveCard()
        {
            var ret = new List<CreditAccount>
            {
                list.Find(x => x.Card.IsCurrent)
            };

            return ret;
        }

        #endregion
    }
}
