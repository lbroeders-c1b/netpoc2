using System.IO;
using System.Security.Cryptography;

using CreditOne.Microservices.BuildingBlocks.EncryptionInformation;

namespace CreditOne.Microservices.BuildingBlocks.Encryption
{
    /// <summary>
    /// This class is used to retrieve encrypted connection strings information from the configuration file. This class is the one that makes the encryptions and decryptions.
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
    public class Crypto
    {
        #region Global Variables from EncryptionInfo
        protected readonly byte[] salt = EncryptionInfo.EncryptionSalt;
        protected readonly byte[] initVector = EncryptionInfo.EncryptionIV;
        protected readonly int passwordLength = EncryptionInfo.PasswordLength;
        protected readonly int numberOfIterations = EncryptionInfo.EncryptionIterations;
        protected readonly string key = EncryptionInfo.EncryptionKey;

        private CryptoBasics myCrypto = new CryptoBasics();
        #endregion

        /// <summary>
        /// This method will encrypt the <paramref name="dataToEncrypt"/> key using all the data from EncryptionInfo.cs
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
        ///     <term>ClearText encryption project.</term>
        ///     <term>Initial Implementation</term>
        /// </item>
        /// </list>
        /// </remarks>
        /// /// <param name="dataToEncrypt">Value of the key that will be encrypted.</param>
        public virtual string EncryptIt(string dataToEncrypt)
        {
            #region Create a new password with intergrated Salt with specified iterations
            Rfc2898DeriveBytes genKey = new Rfc2898DeriveBytes(key, salt, numberOfIterations);
            #endregion

            #region Perform Crypto Operation and return result
            byte[] intermediateData;
            byte[] pseudoKey;

            try
            {
                pseudoKey = (byte[])genKey.GetBytes(passwordLength);
                intermediateData = myCrypto.EncryptIt(dataToEncrypt, pseudoKey, initVector);
            }
            catch
            {
                throw;
            }

            return System.Convert.ToBase64String(intermediateData);
            #endregion
        }

        /// <summary>
        /// This method will decrypt the <paramref name="dataToUnEncrypt"/> key using all the data from EncryptionInfo.cs
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
        ///     <term>ClearText encryption project.</term>
        ///     <term>Initial Implementation</term>
        /// </item>
        /// </list>
        /// </remarks>
        /// /// <param name="dataToUnEncrypt">value of the key that will be dectypted.</param>
        public virtual string UnEncryptIt(string dataToUnEncrypt)
        {
            #region Create a new password with intergrated Salt with specified iterations
            Rfc2898DeriveBytes genKey = new Rfc2898DeriveBytes(key, salt, numberOfIterations);
            #endregion

            #region Perform Crypto Operation and return result
            byte[] arrayedData = System.Convert.FromBase64String(dataToUnEncrypt);

            return myCrypto.UnEncryptIt(arrayedData, genKey.GetBytes(passwordLength), initVector);

            #endregion
        }
    }

    #region These classes should not be used outside of the GlobalUtils assembly
    /// <summary>
    /// This internal class is the one that generate the encrypted keys and decrypt those too. This is the algorithm that we use.
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
    internal class CryptoBasics
    {
        #region Global Variables
        protected readonly int KeySize = 256;
        protected readonly int BlockSize = 128;
        #endregion

        /// <summary>
        /// This method will encrypt the <paramref name="dataToEncrypt"/> key Calling the algorithm
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
        ///     <term>ClearText encryption project.</term>
        ///     <term>Initial Implementation</term>
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="dataToEncrypt">value of the key to encrypt.</param>
        /// <param name="key">key from EncryptionInfo.cs</param>
        /// <param name="IV">initVector from EncryptionInfo.cs</param>
        public virtual byte[] EncryptIt(string dataToEncrypt, byte[] key, byte[] IV)
        {
            using (System.Security.Cryptography.RijndaelManaged myAes = new RijndaelManaged())
            //using (AesManaged myAes = new AesManaged())
            {
                myAes.KeySize = KeySize;
                myAes.BlockSize = BlockSize;
                myAes.Key = key;
                myAes.IV = IV;
                myAes.Mode = System.Security.Cryptography.CipherMode.CBC;
                myAes.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

                ICryptoTransform encryptor = null;
                try
                {
                    encryptor = myAes.CreateEncryptor(key, IV);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    throw new System.Security.Cryptography.CryptographicException("Could not encrypt data with key and IV provided.", ex);
                }

                return ProcessEcStream(dataToEncrypt, encryptor);
            }
        }

        /// <summary>
        /// This method will encrypt the <paramref name="dataToEncrypt"/> key using the encryptor
        /// and the algorithm itself.
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
        ///     <term>ClearText encryption project.</term>
        ///     <term>Initial Implementation</term>
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="dataToEncrypt">value of the key to encrypt.</param>
        /// <param name="encryptor">the class that encrypt and decrypt strings </param>
        protected virtual byte[] ProcessEcStream(string dataToEncrypt, ICryptoTransform encryptor)
        {
            byte[] encrypted = null;

            using (MemoryStream encryptStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(encryptStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter writeStream = new StreamWriter(cryptoStream))
                    {
                        writeStream.Write(dataToEncrypt);
                    }
                    encrypted = encryptStream.ToArray();
                }
            }


            return encrypted;
        }

        /// <summary>
        /// This method will decrypt the <paramref name="dataToUnEncrypt"/> key Calling the algorithm
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
        ///     <term>ClearText encryption project.</term>
        ///     <term>Initial Implementation</term>
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="dataToUnEncrypt">value of the key to decrypt.</param>
        /// <param name="key">key from EncryptionInfo.cs</param>
        /// <param name="IV">initVector from EncryptionInfo.cs</param>
        public virtual string UnEncryptIt(byte[] dataToUnEncrypt, byte[] key, byte[] IV)
        {
            using (System.Security.Cryptography.RijndaelManaged myAes = new RijndaelManaged())
            //using (AesManaged myAes = new AesManaged())
            {
                myAes.KeySize = KeySize;
                myAes.BlockSize = BlockSize;
                myAes.Key = key;
                myAes.IV = IV;
                myAes.Mode = System.Security.Cryptography.CipherMode.CBC;
                myAes.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                ICryptoTransform decryptor = null;

                try
                {
                    decryptor = myAes.CreateDecryptor(myAes.Key, myAes.IV);
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    throw new System.Security.Cryptography.CryptographicException("Could not create decryptor based on key and IV provided. Are the values used to create the encrypted data the same as the values passed to the decryption process?", ex);
                }

                return ProcessUnEcStream(dataToUnEncrypt, decryptor);
            }

        }

        /// <summary>
        /// This method will decrypt the <paramref name="dataToUnEncrypt"/> key using the encryptor
        /// and the algorithm itself.
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
        ///     <term>ClearText encryption project.</term>
        ///     <term>Initial Implementation</term>
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="dataToUnEncrypt">value of the key to decrypt</param>
        /// <param name="encryptor">the class that encrypt and decrypt strings </param>
        protected virtual string ProcessUnEcStream(byte[] dataToUnEncrypt, ICryptoTransform decryptor)
        {
            using (MemoryStream memorystreamDecrypt = new MemoryStream(dataToUnEncrypt))
            {
                using (CryptoStream cryptostreamDecrypt = new CryptoStream(memorystreamDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamreaderDecrypt = new StreamReader(cryptostreamDecrypt))
                    {
                        return streamreaderDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
    #endregion
}
