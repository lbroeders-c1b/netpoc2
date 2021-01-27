using CreditOne.Microservices.BuildingBlocks.Encryption;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace CreditOne.Microservices.BuildingBlocks.MaskEncryptSensitiveData
{
    public static class MaskEncryptSensitiveData
    {
        #region Private Constants

        /// <summary>
        /// Constants for credit card and social security number regular expressions
        /// </summary>
        private const string CreditCardRegEx = @"(?<CreditCard>(?:(?:4[0-9]|5[1-5])\d{2})(?:[ -]?\d{4}){3}|(?:(?:3[47])\d{2})(?:[ -]?\d{6})(?:[ -]?\d{5}))";
        private const string SocialSecurityNumberRegEx = @"^.*(?<SocialSecurityNumber>(?:[-\s]?[0-9]{3})(?:[-\s]?[0-9]{2})(?:[-\s]?[0-9]{4})).*$";

        private const string CreditCardFormatRegEx = @"^(?:(?:4[0-9]|5[1-5])\d{2})(?:[ -]?\d{4}){3}$|^(?:(?:3[47])\d{2})(?:[ -]?\d{6})(?:[ -]?\d{5})$";
        private const string ConnectionStringRegEx = @"\b(?:(User\sId=\w+);(Password=\w+;))";

        private const string FormatCreditCardDefault = "XXXX-XXXX-XXXX-####";
        private const string FormatCreditCardAmex = "XXXX-XXXXXX-X####";
        private const string FormatSocialSecurityNumber = "XXX-XX-####";

        private const string GroupCreditCard = "CreditCard";
        private const string GroupSocialSecurityNumber = "SocialSecurityNumber";

        #endregion

        #region Public Methods

        /// <summary>
        /// Masks and encrypt a credit number or SSN if any is found in the given string.
        /// </summary>
        /// <param name="message">String message to be checked.</param>
        /// <returns>The message with the CCN or SSN encryted and masked</returns>
        public static string MaskAndEncryptSensitiveData(this string message)
        {
            message = MaskAndEncryptSensitiveData(message, CreditCardRegEx, GroupCreditCard);

            message = MaskAndEncryptSensitiveData(message, SocialSecurityNumberRegEx, GroupSocialSecurityNumber);

            message = MaskSensitiveData(message, ConnectionStringRegEx, string.Empty);

            return message;
        }

        /// <summary>
        /// Masks a credit number or SSN if any is found in the given string.
        /// </summary>
        /// <param name="message">String message to be checked.</param>
        /// <returns>The message with the CCN or SSN encryted and masked</returns>
        public static string MaskSensitiveData(this string message)
        {
            message = MaskSensitiveData(message, CreditCardRegEx, GroupCreditCard);

            message = MaskSensitiveData(message, SocialSecurityNumberRegEx, GroupSocialSecurityNumber);

            return message;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generic method to check the existence of a CCN or SSN, using a Regular Expresion, then masks it and encrypts it.
        /// </summary>
        /// <param name="message">String message to be checked.</param>
        /// <param name="regEx">Regular Expresion to be used.</param>
        /// <param name="group">Name of the group of the matching value.</param>
        /// <returns>The message with the CCN or SSN encryted and masked</returns>
        private static string MaskAndEncryptSensitiveData(string message, string regEx, string group)
        {
            Regex regex = new Regex(regEx);
            MatchCollection collection = regex.Matches(message);

            foreach (Match match in collection)
            {
                string potentialMatch = match.Groups[group].Value;
                string formatedValue = potentialMatch.Replace(" ", "").Replace("-", "");

                string encryptedValue = new Crypto().EncryptIt(formatedValue);
                string maskedValue = Masking(group, potentialMatch, formatedValue);

                message = message.Replace(potentialMatch, $"{maskedValue} ({encryptedValue})");
            }

            return message;
        }

        /// <summary>
        /// Generic method to check the existence of a CCN or SSN, using a Regular Expresion, then masks it.
        /// </summary>
        /// <param name="message">String message to be checked.</param>
        /// <param name="regEx">Regular Expresion to be used.</param>
        /// <param name="group">Name of the group of the matching value.</param>
        /// <returns>The message with the CCN or SSN encryted and masked</returns>
        private static string MaskSensitiveData(string message, string regEx, string group)
        {
            Regex regex = new Regex(regEx);
            MatchCollection collection = regex.Matches(message);

            foreach (Match match in collection)
            {
                string potentialMatch = match.Groups[group].Value;
                string formatedValue = potentialMatch.Replace(" ", "").Replace("-", "");

                string maskedValue = Masking(group, potentialMatch, formatedValue);

                message = message.Replace(potentialMatch, $"{maskedValue}");
            }

            return message;
        }

        private static string Masking(string group, string potentialMatch, string formatedValue)
        {
            string maskedValue = string.Empty;

            // This section is only for card numbers
            if (group.Equals(GroupCreditCard) && IsCardNumberValid(potentialMatch))
            {
                if (formatedValue.Length == 15)
                {
                    maskedValue = PerformMasking(FormatCreditCardAmex, formatedValue);
                }
                else
                {
                    maskedValue = PerformMasking(FormatCreditCardDefault, formatedValue);
                }
            }
            else if (group.Equals(GroupSocialSecurityNumber))
            {
                maskedValue = PerformMasking(FormatSocialSecurityNumber, formatedValue);
            }

            return maskedValue;
        }

        /// <summary>
        /// Method to mask a value. 
        /// </summary>
        /// <param name="value">Value to mask</param>
        /// <returns>Value masked</returns>
        private static string PerformMasking(string format, string value)
        {
            StringBuilder maskedValue = new StringBuilder();
            int nextPos = 0;
            for (int i = 0; i < format.Length; i++)
            {
                if (nextPos >= value.Length)
                {
                    throw new FormatException($"Format specified position {nextPos} not expected");
                }

                switch (format[i])
                {
                    case '#':
                        {
                            //Unmask at the position
                            maskedValue.Append(value[nextPos++]);
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
                            maskedValue.Append(format[i]);
                            break;
                        }
                    default:
                        {
                            // replace with character at specifier and move to next position
                            maskedValue.Append(format[i]);
                            nextPos++;
                            break;
                        }
                }
            }

            if (nextPos != value.Length)
            {
                // do not allow unintentional partial format
                throw new FormatException("Not all digit format specifiers were provided");
            }

            return maskedValue.ToString();
        }

        /// <summary>
        /// Checks if card number is valid
        /// </summary>
        /// <remarks>
        /// <list type="table">
        /// <param name="cardNumber">the card number</param>
        /// <returns>True if the card number is valid after performing a mod 10 check</returns>
        private static bool IsCardNumberValid(string cardNumber)
        {
            // Verify the card number is of appropriate length and format
            if (!IsValidCardNumberFormat(cardNumber))
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

        /// <summary>
        /// Validates that the card number is 15 or 16 digits and follows the expected vendor format.
        /// </summary>
        /// <param name="cardNumber">The card number to validate</param>
        private static bool IsValidCardNumberFormat(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                return false;

            if (!IsCardNumberFormatValid(cardNumber))
                return false;

            return true;
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
        private static bool IsCardNumberFormatValid(string cardNumber) => Regex.IsMatch(cardNumber, CreditCardFormatRegEx);

        #endregion
    }
}
