using System;
using System.Text;
using System.Text.RegularExpressions;

namespace CreditOne.Microservices.BuildingBlocks.Types
{
    /// <summary>
    /// Represents a Social Security Number 
    /// Can be masked using format strings and implicitly cast to/from a string.
	/// Decimal conversions are not supported because some SocialSecurity numbers start with zero's 
    /// Should be used instead of a string type of SocialSecurity as property in Middle Tier DataTransports 
    /// 
    /// Xml Serializer for this type is not supported. If needed both ISerializable and IXmlSerializable can be overridden to provide a 
    /// consistent behavior (SOAP/Binary formatters serialize public and private properties allowing for default(SocialSecurity), Xml serializer requires public properties)
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
    ///		<term>12/16/2014</term>
    ///		<term>Ali Toral</term>
    ///		<term>IT-226</term>
    ///		<description>Copied from CreditOne.Middleware.Types.
    ///     Also converted from struct to class (to prevent value-type/reference-type conversions
    ///     when implementing interface on a struct) must be initialized to prevent null reference exceptions                    
    ///     </description>
    /// </item>    
    /// <item>
    ///		<term>10/07/2019</term>
    ///		<term>Luis Petitjean</term>
    ///		<term>RM-47</term>
    ///		<description>Copied from <c>CreditOne.Middleware.Types.SocialSecurityNumber</c></description>
    /// </item>   
    /// </list>
    /// </remarks>
    [Serializable]
    public class SocialSecurityNumber : IFormattable
    {
        private string _value = string.Empty;

        #region CTOR

        /// <summary>
        /// Default CTOR 
        /// </summary>
        public SocialSecurityNumber()
        {
            _value = string.Empty;
        }

        /// <summary>
        /// Converts the string value to a SocialSecurityNumber instance.
        /// string value must be a valid ssn otherwise exception thrown depending on throwInvalidCastExceptionOnFailure
        /// </summary> 
        /// <param name="value">A 9-digit (numbers only) Social Security Number</param>
        /// <param name="argumentName">parameter name to be used in exception if applicable</param>
        /// <param name="throwInvalidCastExceptionOnFailure">if true InvalidCastException otherwise ArgumentException is used on failure convert ssn</param>
        public SocialSecurityNumber(string value)
        {
            if (!IsValidSocialSecurityNumber(value))
                throw new ArgumentException(string.Format("[{0}] is not a valid social security number.", value));

            _value = value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets last four digits of the Social Security Number
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
        ///		<term>12/01/2014</term>
        ///		<term>Casey Barnes</term>
        ///		<term>IT-226</term>
        ///		<description>Modified to not throw an exception if value is not populated.</description>
        /// </item>
        /// </list>
        /// </remarks> 
        public string LastFour
        {
            get
            {
                return (string.IsNullOrEmpty(this._value)) ?
                            default(string) :
                            Regex.Match(this._value, @".{4}$").Value;
            }
        }

        /// <summary>
        /// Gets all digits of the Social Security Number
        /// </summary>
        public string Full
        {
            get { return this._value; }
        }

        #endregion

        #region Equals and GetHashCode methods
        /// <summary>
        /// Determines whether the specified Object is equal to the current instance. 
        /// </summary>
        /// <param name="value">The object to compare with the current object.</param>
        /// <returns>true if equal</returns>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>10/29/2014</term>
        ///		<term>Casey Barnes</term>
        ///		<term>IT-226</term>
        ///		<description>Initial Implementation</description>
        /// </item>
        /// </list>
        /// </remarks>
        public override bool Equals(object value)
        {
            // If parameter is null return false.
            if (value == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            SocialSecurityNumber p = value as SocialSecurityNumber;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return string.Equals(this._value, p._value);
        }

        /// <summary>
        /// Generate a number (hash code) that corresponds to the value of this instance
        /// </summary>
        /// <returns>A hash code for this instance</returns>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>10/29/2014</term>
        ///		<term>Casey Barnes</term>
        ///		<term>IT-226</term>
        ///		<description>Initial Implementation</description>
        /// </item>
        /// </list>
        /// </remarks>
        public override int GetHashCode()
        {
            return string.IsNullOrEmpty(this._value) ? 0 : this._value.GetHashCode();
        }
        #endregion

        #region ToString Methods

        /// <summary>
        /// Format the value into a string representation using default format specifier XXX-XX-####.
        /// </summary>
        /// <returns>if has value formatted string, otherwise default(string) which is null        
        /// <example>XXX-XX-3456</example>                
        /// </returns>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>08/10/2010</term>
        ///		<term>Sherzod Niazov</term>
        ///		<term>IT-208</term>
        ///		<description>Initial Implementation</description>
        /// </item>
        /// <item>
        ///		<term>10/29/2014</term>
        ///		<term>Casey Barnes</term>
        ///		<term>IT-226</term>
        ///		<description>Changed to use Masked Format instead of just default to last four</description>
        /// </item>
        /// </list>
        /// </remarks>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// Format the value into a string representation.
        /// </summary>
        /// <param name="format">Specifier for all digits must be provided.
        /// <example>XXX-XX-####</example>                
        ///</param>
        /// <returns>if has value formatted string, otherwise default(string) which is null</returns>        
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>10/29/2014</term>
        ///		<term>Casey Barnes</term>
        ///		<term>IT-226</term>
        ///		<description>Initial Implementation</description>
        /// </item>
        /// </list>
        /// </remarks>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        #endregion

        #region IFormattable Members
        /// <summary>
        /// Format the value into a string representation.
        /// 
        /// </summary>
        /// <param name="format">Specifier for all 9 digits must be provided.
        /// <example>XXX-XX-####</example>        
        /// <list type="table">
        /// <listheader>
        ///		<term>Specifier</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>#</term>
        ///		<description>Show the number at position</description>
        /// </item>
        /// <item>
        ///		<term>S</term>
        ///		<description>Skip  the number at position</description>
        /// </item>
        /// <item>
        ///		<term>-</term>
        ///		<description>Inserts - character without skipping position</description>
        /// </item>
        /// <item>
        ///		<term>X (or any other character)</term>
        ///		<description>Replace the number at position with this character</description>
        /// </item>
        /// </list>
        ///</param>
        /// <param name="formatProvider"><see cref="SocialSecurityNumberFormat"/></param>
        /// <returns>if has value formatted string, otherwise default(string) which is null</returns>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>10/29/2014</term>
        ///		<term>Casey Barnes</term>
        ///		<term>IT-226</term>
        ///		<description>Initial Implementation</description>
        /// </item>
        /// </list>
        /// </remarks>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (formatProvider != null)
            {
                // custom formatter might support our type
                ICustomFormatter formatter = formatProvider.GetFormat(this.GetType()) as ICustomFormatter;
                if (formatter != null)
                {
                    return formatter.Format(format, this, formatProvider);
                }
            }

            return FormatSocialSecurityNumber(format, this._value);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Validates a Social Security Number
        /// </summary>
        /// <param name="value">A 9-digit (numbers only) Social Security Number</param>
        public static bool IsValidSocialSecurityNumber(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            if (!Regex.IsMatch(value, @"^[0-9]{9}$"))
                return false;

            return true;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Format the value into a string representation.        
        /// </summary>
        /// <param name="format">Specifier for all 9 digit must be provided.
        /// <example>XXX-XX-####</example>                
        ///</param>
        /// <param name="formatProvider"><see cref="SocialSecurityNumberFormat"/></param>
        /// <returns>if has value formatted string, otherwise default(string) which is null</returns>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>10/29/2014</term>
        ///		<term>Casey Barnes</term>
        ///		<term>IT-226</term>
        ///		<description>Initial Implementation</description>
        /// </item>
        /// </list>
        /// </remarks>
        private string FormatSocialSecurityNumber(string format, string ssnValue)
        {
            if (string.IsNullOrEmpty(ssnValue))
            {
                return default(string);
            }
            else if ((format == null) || string.Equals(format, "G", StringComparison.InvariantCultureIgnoreCase))
            {
                // http://msdn.microsoft.com/en-us/library/system.iformattable.aspx
                // A class that implements IFormattable must support the "G" (general) format specifier. 
                // Besides the "G" specifier, the class can define the list of format specifiers that it supports. 
                // In addition, the class must be prepared to handle a format specifier that is null.
                // 
                // default format
                format = "XXX-XX-####";
            }

            StringBuilder result = new StringBuilder();
            int nextPos = 0;
            for (int i = 0; i < format.Length; i++)
            {
                if (nextPos >= ssnValue.Length)
                {
                    throw new FormatException(string.Format("Format specified position {0} not expected", nextPos));
                }

                switch (format[i])
                {
                    case '#':
                        {
                            //Unmask at the position
                            result.Append(ssnValue[nextPos++]);
                            break;
                        }
                    case 'S':
                        {
                            // skip position
                            nextPos++;
                            break;
                        }
                    case '-':
                        {
                            // insert character without skip position
                            result.Append(format[i]);
                            break;
                        }
                    default:
                        {
                            // replace with character at specifier and move to next position
                            result.Append(format[i]);
                            nextPos++;
                            break;
                        }
                }
            }

            if (nextPos != ssnValue.Length)
            {
                // do not allow unintentional partial format
                throw new FormatException("Not all digit format specifiers were provided");
            }
            return result.ToString();
        }

        #endregion

        #region IXmlSerializable Members
        /// <summary>
        /// GetSchema for Xml Serializer
        /// NOT Supported
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>10/29/2014</term>
        ///		<term>Casey Barnes</term>
        ///		<term>IT-226</term>
        ///		<description>Initial Implementation</description>
        /// </item>
        /// </list>
        /// </remarks>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            // Will not allow for Xml serialization due to unmasked ssn exposure
            throw new InvalidOperationException(string.Format("Xml serializer for this type {0} is not supported. Please use Binary or Soap formatter serialization.",
                                                this.GetType().FullName));
        }

        /// <summary>
        /// Deserialize from Xml.
        /// NOT Supported
        /// </summary>
        /// <param name="reader"></param>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>10/29/2014</term>
        ///		<term>Casey Barnes</term>
        ///		<term>IT-226</term>
        ///		<description>Initial Implementation</description>
        /// </item>
        /// </list>
        /// </remarks>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            // Do not allow for Xml serialization due to unmasked ssn exposure
            throw new InvalidOperationException(string.Format("Xml de-serialization of this type {0} is not supported. Please use Binary or Soap formatter serialization.",
                                                this.GetType().FullName));
        }

        /// <summary>
        /// Serialize to xml
        /// NOT Supported
        /// </summary>
        /// <param name="writer"></param>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>10/29/2014</term>
        ///		<term>Casey Barnes</term>
        ///		<term>IT-226</term>
        ///		<description>Initial Implementation</description>
        /// </item>
        /// </list>
        /// </remarks>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            // Do not allow for Xml serialization due to unmasked ssn exposure
            throw new InvalidOperationException(string.Format("Xml serialization of this type {0} is not supported. Please use Binary or Soap formatter serialization.",
                                                this.GetType().FullName));
        }
        #endregion

        #region IMaskable Members

        /// <summary>
        /// Returns default formatted masked value
        /// </summary>
        /// <returns>Masked value</returns> 
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>10/29/2014</term>
        ///		<term>Casey Barnes</term>
        ///		<term>IT-226</term>
        ///		<description>Initial Implementation</description>
        /// </item>
        /// </list>
        /// </remarks>
        public string GetMaskedValue()
        {
            return this.ToString();
        }

        /// <summary>
        /// Returns default unmasked value
        /// </summary>
        /// <returns>Unmasked value</returns> 
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>10/29/2014</term>
        ///		<term>Casey Barnes</term>
        ///		<term>IT-226</term>
        ///		<description>Initial Implementation</description>
        /// </item>
        /// </list>
        /// </remarks>
        public string GetUnMaskedValue()
        {
            return this.Full;
        }

        #endregion
    }
}