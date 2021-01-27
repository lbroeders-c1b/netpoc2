using System;
using System.Text;
using System.Text.RegularExpressions;

namespace CreditOne.Microservices.BuildingBlocks.Types
{
    /// <summary>
    /// Represents a CreditCard Number. 
    /// Can be masked using format strings and implicitly cast to/from a string and decimal.
    /// Should be used instead of a string type of CreditCardNumber as property in Middle Tier DataTransports 
    /// 
    /// Xml Serializer for this type is not supported. If needed both ISerializable and IXmlSerializable can be overridden to provide a 
    /// consistent behavior (SOAP/Binary formatters serialize public and private properties allowing for default(BankCardNumber), Xml serializer requires public properties)
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
    ///		<description>Copied from <c>CreditOne.CoreLibrary.Types</c></description>
    /// </item>   
    /// </list>
    /// </remarks>
    [Serializable]
    public class BankCardNumber : IFormattable
    {
        private string _cardNumberValue = string.Empty;

        #region CTOR

        /// <summary>
        /// Default CTOR 
        /// </summary>
        public BankCardNumber()
        {
            _cardNumberValue = string.Empty;
        }

        /// <summary>
        /// Converts the decimal value to a BankCardNumber instance.
        /// decimal value must be a valid card number otherwise <see cref="ArgumentException"/> is thrown
        /// </summary>
        /// <param name="cardNumber">cardNumber to be converted to BankCardNumber</param>            
        /// <example>BankCardNumber bankCardNumber = new BankCardNumber(decimalCardNumber);</example>
        public BankCardNumber(decimal cardNumber)
            : this(cardNumber.ToString())
        {
        }

        /// <summary>
        /// Converts the string value to a BankCardNumber instance.
        /// string value must be a valid card number otherwise exception thrown depending on throwInvalidCastExceptionOnFailure
        /// </summary>                
        /// <param name="cardNumber">cardNumber to be converted to BankCardNumber</param>
        /// <param name="argumentName">parameter name to be used in exception if applicable</param>
        /// <param name="throwInvalidCastExceptionOnFailure">if true InvalidCastException otherwise ArgumentException is used on failure convert cardnumber</param>
        public BankCardNumber(string cardNumber)
        {
            if (!IsValidCardNumber(cardNumber))
                throw new ArgumentException(string.Format("[{0}] is not a valid bank card number.", cardNumber));

            this._cardNumberValue = cardNumber;
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Converts the card number value of BankCardNumber instance to a string and returns last four characters. 
        /// </summary>
        /// <example>string bankCardNumberLastFour = bankCardNumber.LastFour;</example>
        /// <returns>if has a value converted to string and last four digits, otherwise default(string) which is null</returns>
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
        ///		<term>Ali Toral</term>
        ///		<term>IT-208</term>
        ///		<description></description>
        /// </item>
        /// </list>
        /// </remarks> 
        public string LastFour
        {
            get
            {
                // we could use return this.ToString("SSSSSSSSSSSS####"); 
                // but below is more flexible and in the future we could support other card number types such as amex... that might have different number of digits
                return (string.IsNullOrEmpty(this._cardNumberValue)) ?
                            default(string) :
                            Regex.Match(this._cardNumberValue, @".{4}$").Value;
            }
        }

        /// <summary>
        /// Converts the card number value of BankCardNumber instance to a string and returns the full number. 
        /// </summary>
        /// <example>string bankCardNumber = bankCardNumber.Full;</example>
        /// <returns>if has a value converted to string, otherwise default(string) which is null</returns>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>09/6/2010</term>
        ///		<term>BJ Perna</term>
        ///		<term>IT-208</term>
        ///		<description></description>
        /// </item>
        /// </list>
        /// </remarks> 
        public string Full
        {
            get
            {
                return this._cardNumberValue;
            }
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
        ///		<term>08/10/2010</term>
        ///		<term>Ali Toral</term>
        ///		<term>IT-208</term>
        ///		<description></description>
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
            BankCardNumber p = value as BankCardNumber;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return string.Equals(this._cardNumberValue, p._cardNumberValue);
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
        ///		<term>08/10/2010</term>
        ///		<term>Ali Toral</term>
        ///		<term>IT-208</term>
        ///		<description></description>
        /// </item>
        /// </list>
        /// </remarks>
        public override int GetHashCode()
        {
            return string.IsNullOrEmpty(this._cardNumberValue) ? 0 : this._cardNumberValue.GetHashCode();
        }
        #endregion

        #region ToString Methods
        /// <summary>
        /// Format the value into a string representation using default format specifier XXXX-XXXX-XXXX-####.
        /// </summary>
        /// <returns>if has value formatted string, otherwise default(string) which is null        
        /// <example>XXXX-XXXX-XXXX-3456</example>                
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
        ///		<term>Ali Toral</term>
        ///		<term>IT-208</term>
        ///		<description></description>
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
        /// <example>XXXX-XXXX-XXXX-####</example>                
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
        ///		<term>08/10/2010</term>
        ///		<term>Ali Toral</term>
        ///		<term>IT-208</term>
        ///		<description></description>
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
        /// <param name="format">Specifier for all 16 digit must be provided.
        /// <example>XXXX-XXXX-XXXX-####</example>        
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
        /// <param name="formatProvider"><see cref="BankCardNumberFormat"/></param>
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
        ///		<term>08/10/2010</term>
        ///		<term>Ali Toral</term>
        ///		<term>IT-208</term>
        ///		<description></description>
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

            return FormatCardNumber(format, this._cardNumberValue);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Validates that the card number is 15 or 16 digits and follows the expected vendor format.
        /// </summary>
        /// <param name="cardNumber">The card number to validate</param>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>01/23/2019</term>
        ///		<term>RRIOS</term>
        ///		<term>IT-328</term>
        ///		<description>Updated regex string</description>
        /// </item>
        /// </list>
        /// </remarks>
        public static bool IsValidCardNumber(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                return false;

            if (!cardNumber.IsCardNumberFormatValid())
                return false;

            return true;
        }

        /// <summary>
        /// Format the value into a string representation.        
        /// </summary>
        /// <param name="format">Specifier for all 16 digit must be provided.
        /// <example>XXXX-XXXX-XXXX-####</example>                
        ///</param>
        /// <param name="formatProvider"><see cref="BankCardNumberFormat"/></param>
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
        ///		<term>08/10/2010</term>
        ///		<term>Ali Toral</term>
        ///		<term>IT-208</term>
        ///		<description></description>
        /// </item>
        /// <item>
        ///		<term>05/29/2010</term>
        ///		<term>Richard Rios</term>
        ///		<term>IT-328</term>
        ///		<description>Update to support AmEx cards</description>
        /// </item>
        /// </list>
        /// </remarks>
        private string FormatCardNumber(string format, string cardNumberValue)
        {
            if (string.IsNullOrEmpty(cardNumberValue))
            {
                return default(string);
            }

            if (cardNumberValue.Length == 15 && (string.IsNullOrWhiteSpace(format) || !format.Equals("A")))
            {
                format = "A";
            }

            // http://msdn.microsoft.com/en-us/library/system.iformattable.aspx
            // A class that implements IFormattable must support the "G" (general) format specifier. 
            // Besides the "G" specifier, the class can define the list of format specifiers that it supports. 
            // In addition, the class must be prepared to handle a format specifier that is null.
            // 
            // G = general/default format
            // V = Visa
            // M = MasterCard
            // A = American Express
            switch (format)
            {
                case "G":
                case "V":
                case "M":
                    format = "XXXX-XXXX-XXXX-####";
                    break;
                case "A":
                    format = "XXXX-XXXXXX-X####";
                    break;
                default:
                    format = "XXXX-XXXX-XXXX-####";
                    break;
            }

            StringBuilder result = new StringBuilder();
            int nextPos = 0;
            for (int i = 0; i < format.Length; i++)
            {
                if (nextPos >= cardNumberValue.Length)
                {
                    throw new FormatException(string.Format("Format specified position {0} not expected", nextPos));
                }

                switch (format[i])
                {
                    case '#':
                        {
                            //Unmask at the position
                            result.Append(cardNumberValue[nextPos++]);
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

            if (nextPos != cardNumberValue.Length)
            {
                // do not allow unintentional partial format
                throw new FormatException("Not all digit format specifiers were provided");
            }
            return result.ToString();
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

        /// <summary>
        /// Checks if card number is valid
        /// </summary>
        /// <remarks>
        /// <list type="table">
        /// <param name="cardNumber">the card number</param>
        /// <returns>True if the card number is valid after performing a mod 10 check</returns>
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>01/05/2015</term>
        ///		<term>Casey Barnes</term>
        ///		<term>IT-226</term>
        ///		<description>Initial Implementation</description>
        /// </item>
        /// <item>
        ///		<term>09/03/2015</term>
        ///		<term>rmeine</term>
        ///		<term>WO68581</term>
        ///		<description>Moved method to here from FNBM.ApplicationProcessing.
        /// Utilities.Logging.</description>
        /// </item>
        /// <item>
        ///		<term>01/22/2019</term>
        ///		<term>RRIOS</term>
        ///		<term>IT-328</term>
        ///		<description>Updated to use Luhns Algorithm</description>
        /// </item>
        /// </list>
        /// </remarks>
        public static bool IsCardNumberValid(string cardNumber)
        {
            // Verify the card number is of appropriate length and format
            if (!IsValidCardNumber(cardNumber))
            {
                return false;
            }

            cardNumber = cardNumber.Replace(" ", "").Replace("-", "");
            int sum = 0;                        // Counter to keep track of sum of digits
            int digits = cardNumber.Length;     // Card number length
            int parity = (digits - 1) % 2;      // value to determine every other digit in card number, 1 for len(16), 0 for len(15)

            // Begin from right most value of card number
            for (int i = digits; i > 0; i--)
            {
                int digit = int.Parse(cardNumber[i - 1].ToString());

                if (parity == (i % 2))
                {
                    digit *= 2;
                }

                sum += digit / 10;
                sum += digit % 10;
            }

            return (sum % 10 == 0);
        }
    }
}