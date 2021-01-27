namespace CreditOne.Microservices.BuildingBlocks.Common.Domain
{
    /// <summary>
    /// This class implements the RelationsTypeEnum class.
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
	///		<term>5/24/2019 7:06:39 AM</term>
	///		<term>arsoto</term>
	///		<term>RM-47</term>
	///		<description>Initial implementation</description>
	/// </item>
    /// </list>
    /// </remarks> 	
    public class RelationsTypeEnum : Enumeration
    {
        #region Constructors

        /// <summary>
        /// Constructor for the class RelationsTypeEnum.
        /// </summary>      
        public RelationsTypeEnum(string id, string name) : base(id, name)
        {
        }

        #endregion

        #region Public Properties

        public static RelationsTypeEnum Addresses = new RelationsTypeEnum("Addresses", "Addresses");
        public static RelationsTypeEnum Cards = new RelationsTypeEnum("Cards", "Cards");
        public static RelationsTypeEnum PrimaryCardHolder = new RelationsTypeEnum("PrimaryCardHolder", "PrimaryCardHolder");

        #endregion
    }
}
