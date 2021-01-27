using System;

namespace CreditOne.Microservices.BuildingBlocks.Types
{
    // TODO: expand functionality, add operators, and First, Last name parsing etc

    /// <summary>
    /// Represents a name of a person
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
    ///		<term>08/03/2012</term>
    ///		<term>Sherzod Niazov</term>
    ///		<term>IT-208</term>
    ///		<description>Initial implementation</description>
    /// </item>
    /// <item>
    ///		<term>10/07/2019</term>
    ///		<term>Luis Petitjean</term>
    ///		<term>RM-47</term>
    ///		<description>Copied from <c>CreditOne.Middleware.Types.PersonName</c></description>
    /// </item>
    /// </list>
    /// </remarks>
    public struct PersonName
    {

        /// <summary>
        /// Creates a new PersonName.
        /// </summary>
        /// <param name="fdrName">The FDR Name format</param>
        public PersonName(string fdrName)
        {
            if (string.IsNullOrEmpty(fdrName))
                throw new ArgumentException("fdrName");

            this._fdrName = fdrName;
            this._firstName = fdrName.Split(new char[] { ',' })[1].Trim();
            this._lastName = fdrName.Split(new char[] { ',' })[0].Trim();
        }

        public PersonName(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName))
                throw new ArgumentException("firstName");
            if (string.IsNullOrEmpty(lastName))
                throw new ArgumentException("lastName");

            this._firstName = firstName;
            this._lastName = lastName;
            this._fdrName = string.Format("{0},{1}", lastName, firstName);
        }

        private string _fdrName;
        /// <summary>
        /// Gets name in FDR Format which is Last,First
        /// </summary>
        public string FdrName
        {
            get { return _fdrName; }
        }

        private string _firstName;
        public string FirstName
        {
            get { return this._firstName; }
            set { this._firstName = value; }
        }

        private string _lastName;
        public string LastName
        {
            get { return this._lastName; }
            set { this._lastName = value; }
        }

        public string FullName
        {
            get { return string.Format("{0} {1}", this._firstName, this._lastName); }
        }
    }
}