namespace CreditOne.Microservices.BuildingBlocks.EncryptionInformation
{
    /// <summary>
    /// Represents the Encryption Information object. Those are all the needed values for encrypting and decrypting.
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///     <term>Date</term>
    ///     <term>Who</term>
    ///     <term>BR/WO</term>
    ///     <term>Description</term>
    /// </listheader>
    /// <item>
    ///     <term>5/5/2016</term>
    ///     <term>Dennis Moriarity</term>
    ///     <term>Encrypt ClearText information</term>
    ///     <term>Initial Implementation</term>
    /// </item>
    /// </list>
    /// </remarks>
    public static class EncryptionInfo
    {
        #region properties

        private static string _encryptionKey = "C1BEncryptionKey";

        private static int _passwordLength = 32;

        // When modifying the vector byte array, make sure to include exactly 16 members in the byte array.
        private static byte[] _encryptionIV = new byte[] { 21, 24, 63, 9, 7, 123, 12, 22, 15, 82, 50, 21, 73, 42, 94, 96 };

        // When modifying the salt byte array, make sure to include exactly 16 members in the byte array.
        private static byte[] _encryptionSalt = new byte[] { 22, 44, 66, 8, 5, 122, 10, 28, 12, 88, 54, 24, 76, 45, 99, 97 };

        private static int _encryptionIterations = 7;

        /// <summary>
        /// Read Only. Gets the unique key for the encryption information.
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
        ///		<term>05/05/2016</term>
        ///		<term>Dennis Moriarity</term>
        ///		<term>Encrypt ClearText information</term>
        ///		<description>Initial Implementation</description>
        /// </item>
        /// </list>
        /// </remarks>
        public static string EncryptionKey
        {
            get { return _encryptionKey; }
        }

        /// <summary>
        /// Read Only. Gets the maximum password Length.
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
        ///		<term>05/05/2016</term>
        ///		<term>Dennis Moriarity</term>
        ///		<term>Encrypt ClearText information</term>
        ///		<description>Initial Implementation</description>
        /// </item>
        /// </list>
        /// </remarks>
        public static int PasswordLength
        {
            get { return _passwordLength; }
        }

        /// <summary>
        /// Read Only. Gets the salt byte array used to construct the encrypted data.
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
        ///		<term>05/05/2016</term>
        ///		<term>Dennis Moriarity</term>
        ///		<term>Encrypt ClearText information</term>
        ///		<description>Initial Implementation</description>
        /// </item>
        /// </list>
        /// </remarks>
        public static byte[] EncryptionSalt
        {
            get { return _encryptionSalt; }
        }

        /// <summary>
        /// Read Only. Gets the vector byte array used to construct the encrypted data.
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
        ///		<term>05/05/2016</term>
        ///		<term>Dennis Moriarity</term>
        ///		<term>Encrypt ClearText information</term>
        ///		<description>Initial Implementation</description>
        /// </item>
        /// </list>
        /// </remarks>
        public static byte[] EncryptionIV
        {
            get { return _encryptionIV; }
        }

        /// <summary>
        /// Read Only. Gets the number of iterations to perform when encrypting the data.
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
        ///		<term>05/05/2016</term>
        ///		<term>Dennis Moriarity</term>
        ///		<term>Encrypt ClearText information</term>
        ///		<description>Initial Implementation</description>
        /// </item>
        /// </list>
        /// </remarks>
        public static int EncryptionIterations
        {
            get { return _encryptionIterations; }
        }

        #endregion
    }
}
