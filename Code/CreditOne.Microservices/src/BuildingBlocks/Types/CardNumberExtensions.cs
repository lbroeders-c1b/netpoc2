using System.Linq;
using System.Text.RegularExpressions;

namespace CreditOne.Microservices.BuildingBlocks.Types
{
    /// <summary>
    /// Extension methods for card numbers to reduce repeated code of common checks.
    /// </summary>
    /// <list type="table">
    /// <remarks>
    /// <listheader>
    ///     <item>Date</item>
    ///     <item>Who</item>
    ///     <item>BR/WO</item>
    ///     <description>Description of Changes</description>
    /// </listheader>
    /// <item>
    ///     <item>03/07/2019</item>
    ///     <item>Richard Rios</item>
    ///     <item>IT-328</item>
    ///     <description>Initial Implementation</description>
    /// </item>
    /// <item>
    ///		<term>10/07/2019</term>
    ///		<term>Luis Petitjean</term>
    ///		<term>RM-47</term>
    ///		<description>Copied from <c>CreditOne.CoreLibrary.Types</c></description>
    /// </item>
    /// </list>
    /// </remarks>
    public static class CardNumberExtensions
    {
        #region Private Constants

        private const int MinimumCardnumber = 15;
        private const int MaximumCardnumber = 19;
        private const int AmexCardnumberLength = 15;
        private const int VisaMasterCardnumberLength = 16;
        private const int FutureVisaAndMasterCardnumberLength = 19;
        private const int SpecialCaseCardnumberLength = 0;

        private const string CardRegex = @"^(?:(?:4[0-9]|5[1-5])\d{2})(?:[ -]?\d{4}){3}$|^(?:(?:3[47])\d{2})(?:[ -]?\d{6})(?:[ -]?\d{5})$";
        private const string VisaCardRegex = @"^4.{15}$";
        private const string AmexCardRegex = @"^5[1-5].{14}$";
        private const string MasterCardRegex = @"^3[47].{13}$";

        #endregion

        /// <summary>
        /// Return the last 4 of a card number. This extension method assumes the card number has been
        /// properly verified.
        /// </summary>
        /// <param name="cardNumber">String representation of card number</param>
        /// <returns>String representing last 4 of card number</returns>
        public static string GetLast4(this string cardNumber) => cardNumber.Length > 4 ? cardNumber.Substring(cardNumber.Length - 4) : cardNumber;

        /// <summary>
        /// Determines whether the card number in question contains only numerical values.
        /// </summary>
        /// <param name="cardNumber">String representing card number</param>
        /// <returns>true if numeric, false otherwise</returns>
        public static bool IsCardNumberNumeric(this string cardNumber)
        {
            // Fail-fast section
            if (string.IsNullOrWhiteSpace(cardNumber)) { return false; }

            bool isNumeric = cardNumber.Trim().All(x => char.IsDigit(x));

            return isNumeric;
        }

        /// <summary>
        /// Return only the numerical value of a card number that contains separators.
        /// </summary>
        /// <param name="cardNumber">string representing card number</param>
        /// <returns>15, 16, or 19 numerical character string</returns>
        public static string GetCardNumericsOnly(this string cardNumber) => new string(cardNumber.Where(x => char.IsDigit(x)).ToArray());

        /// <summary>
        /// Verifies whether the card number length is in range of 15-19 characters
        /// </summary>
        /// <param name="cardNumber">string representing the card number</param>
        /// <returns>true if in range, false otherwise</returns>
        public static bool IsCardLengthInRange(this string cardNumber)
        {
            // Fail-fast section
            if (string.IsNullOrWhiteSpace(cardNumber)) { return false; }

            bool isLengthInRange = (cardNumber.Trim().Length >= MinimumCardnumber && cardNumber.Trim().Length <= MaximumCardnumber);

            return isLengthInRange;
        }

        /// <summary>
        /// This method verifies that the card number format is valid and verifies the length of the supplied card number.
        /// The Regex will match whether the string contains hyphens or not.
        /// 
        ///            CARD NUMBER FORMATS
        ///            -------------------
        ///      VENDOR                 VALID FORMATS
        ///      ------              -------------------
        ///      Visa                4XXX-XXXX-XXXX-XXXX
        ///      Visa                4XXXXXXXXXXXXXXX
        ///       MC                 5[1,2,3,4,5]XX-XXXX-XXXX-XXXX
        ///       MC                 5[1,2,3,4,5]XXXXXXXXXXXXXX
        ///      AmEx                3[4,7]XX-XXXXXX-XXXXX
        ///      AmEx                3[4,7]XXXXXXXXXXXXX
        /// </summary>
        /// <param name="cardNumber">String representing the card number</param>
        /// <returns>true if it's valid, false otherwise</returns>
        public static bool IsCardNumberFormatValid(this string cardNumber) => Regex.IsMatch(cardNumber, CardRegex);

        /// <summary>
        /// Mask the card number with default value or custom value
        /// </summary>
        /// <param name="cardNumber">string representing card number</param>
        /// <param name="maskValue">Defaults to 'X', can be customized to any char</param>
        /// <returns></returns>
        public static string MaskCardNumber(this string cardNumber, char maskValue = 'X')
        {
            string maskedCardNumber = string.Empty;
            string numericCardNumber = cardNumber;

            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                return string.Empty;
            }

            // Remove special characters
            if (!numericCardNumber.IsCardNumberNumeric())
            {
                numericCardNumber = new string(cardNumber.Where(x => char.IsDigit(x)).ToArray());
            }

            // Mask card appropriately
            switch (numericCardNumber.Length)
            {
                // Special case, when card is all X's.
                case SpecialCaseCardnumberLength:
                    maskedCardNumber = cardNumber;
                    break;
                // AmEx
                case AmexCardnumberLength:
                    maskedCardNumber = $"{new string(maskValue, 4)}-{new string(maskValue, 6)}-{maskValue}{numericCardNumber.Substring(numericCardNumber.Length - 4)}";
                    break;
                // Visa & MC
                case VisaMasterCardnumberLength:
                    maskedCardNumber = $"{new string(maskValue, 4)}-{new string(maskValue, 4)}-{new string(maskValue, 4)}-{numericCardNumber.Substring(numericCardNumber.Length - 4)}";
                    break;
                // Future Visa & MC 
                case FutureVisaAndMasterCardnumberLength:
                    maskedCardNumber = $"{new string(maskValue, 4)}-{new string(maskValue, 4)}-{new string(maskValue, 4)}-" +
                                       $"{new string(maskValue, 3)}{numericCardNumber.Substring(numericCardNumber.Length - 4, 1)}-{numericCardNumber.Substring(numericCardNumber.Length - 3)}";
                    break;
                default:
                    string cardSubstring = numericCardNumber.Length > 4 ? numericCardNumber.Substring(numericCardNumber.Length - 4) : numericCardNumber;
                    maskedCardNumber = $"{new string(maskValue, 4)}-{new string(maskValue, 4)}-{new string(maskValue, 4)}-{cardSubstring}";
                    break;
            }

            return maskedCardNumber;
        }

        /// <summary>
        /// Determines card type from the card number
        /// </summary>
        /// <param name="cardNumber">string representing card number</param>
        /// <returns></returns>
        public static CardType GetCardType(this string cardNumber)
        {
            string numericCardNumber = cardNumber.GetCardNumericsOnly();

            if (Regex.IsMatch(numericCardNumber, VisaCardRegex))
            {
                return CardType.Visa;
            }

            if (Regex.IsMatch(numericCardNumber, MasterCardRegex))
            {
                return CardType.Mastercard;
            }

            if (Regex.IsMatch(numericCardNumber, AmexCardRegex))
            {
                return CardType.Amex;
            }

            return CardType.Unknown;
        }
    }

    public enum CardType
    {
        Visa,
        Mastercard,
        Amex,
        Unknown
    }
}