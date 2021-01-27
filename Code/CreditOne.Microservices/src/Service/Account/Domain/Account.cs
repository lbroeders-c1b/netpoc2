namespace CreditOne.Microservices.Account.Domain
{
    /// <summary>
    /// Implements the account domain class
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
    ///		<term>Armando Soto</term>
    ///		<term>RM-47</term>
    ///		<description>Initial implementation</description>
    ///	</item>
    /// </list>
    /// </remarks> 
    public class Account
    {
        #region Private Properties

        private string _phoneFlag;

        #endregion

        #region Public Properties

        public string PrimaryName { get; set; }

        public string SecondaryName { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string InternalStatusCode { get; set; }

        public string ExternalStatus { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string SystemOfRecord { get; set; }

        public string Last4SSN { get; set; }

        public decimal CreditAccountId { get; set; }

        public decimal CustomerId { get; set; }

        public string CardNumber { get; set; }

        public string PhoneFlag
        {
            get
            {
                return _phoneFlag;
            }

            set
            {
                _phoneFlag = value;
            }
        }

        public bool HasBadPhone
        {
            get
            {
                return _phoneFlag == "Y";
            }
        }

        #endregion
    }
}
