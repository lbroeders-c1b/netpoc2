using System;
using System.Collections.Generic;
using System.Configuration;

using CreditOne.Microservices.BuildingBlocks.Encryption;

namespace CreditOne.Microservices.BuildingBlocks.ConfigReader
{
    /// <summary>
    /// This class is used to retrieve encrypted connection strings information from the configuration file. Is the main connection between
    /// all the solutions and Crypto.cs.
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
    public static class ConfigReader
    {
        private static Crypto crypto = new Crypto();

        public static bool EncryptedData { get; set; }

        /// <summary>
        /// GetConnectionString will retrieve the connection string data and unencrypt it from the configuration file. 
        /// This information must be in the <connectionString> section of the configuration file. 
        /// If the data you are trying to retrieve is in the appSettings section use the KeyReader class.
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
        [Obsolete("This method is deprecated, you must use Decrypt.")]
        public static string GetConnectionString(string connectionStringName)
        {
            if (string.IsNullOrEmpty(connectionStringName))
            {
                throw new ArgumentNullException("Connection string name or value must not be null or blank.");
            }

            var connectionStringValue = ConfigurationManager.ConnectionStrings[connectionStringName];

            if (connectionStringValue == null)
            {
                throw new ArgumentNullException("Connection string name or value must not be null or blank.");
            }

            if (IsEncryptionEnabled())
            {
                return crypto.UnEncryptIt(connectionStringValue.ConnectionString);
            }
            else
            {
                return connectionStringValue.ConnectionString;
            }
        }

        /// <summary>
        /// Retrieve the data associated with the <paramref name="keyName"/> in the configuration file under the appSettings section.
        /// It retrieve the data using Crypto.cs if the Encrypted key is true, if it is not it will just use the configurationManager.
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
        /// <param name="keyName">Name of the key in the configuration file.</param>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">This will throw a configuration error exception if the key does not exist in the configuration file.</exception>
        /// <exception cref="System.Encryption.CryptographicException">This will be thrown when there is mismatch between the information used to encrypt the data and unencrypt the data.</exception>
        [Obsolete("This method is deprecated, you must use Decrypt.")]
        public static string GetKeyData(string keyName)
        {
            if (string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentNullException("keyName passed to GetEncryptedKeyData must not be null or blank.");
            }

            var keyData = ConfigurationManager.AppSettings[keyName];

            if (string.IsNullOrEmpty(keyData))
            {
                throw new ConfigurationErrorsException(String.Format("Key: {0} does not exist in configuration file.", keyName));
            }

            if (IsEncryptionEnabled() && !string.IsNullOrEmpty(keyData))
            {
                keyData = crypto.UnEncryptIt(keyData);
            }

            return keyData;
        }

        /// <summary>
        /// Retrieve the data associated with the <paramref name="keyName"/> in the configuration file under the appSettings section.
        /// If the key does not exist in the configuration file return NULL.
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
        /// <param name="keyName">Name of the key in the configuration file.</param>
        /// <exception cref="System.Encryption.CryptographicException">This will be thrown when there is mismatch between the information used to encrypt the data and unencrypt the data.</exception>
        [Obsolete("This method is deprecated, you must use Decrypt.")]
        public static string GetKeyDataWithNullValue(string keyName)
        {
            if (string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentNullException("keyName passed to GetKeyDataWithNull must not be null or blank.");
            }

            var keyData = ConfigurationManager.AppSettings[keyName];

            if (IsEncryptionEnabled() && !string.IsNullOrEmpty(keyData))
            {
                keyData = crypto.UnEncryptIt(keyData);
            }

            return keyData;
        }

        /// <summary>
        /// Retrieve the data associated with the <paramref name="DataSourceName"/> in the configuration file under the dataSource Section section.
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
        /// <param name="data">The datasource in the configuration file.</param>
        [Obsolete("This method is deprecated, you must use Decrypt.")]
        public static string GetDataSource(string data)
        {
            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(data.Trim()))
            {
                throw new ArgumentNullException("Data passed to GetKeyDataWithNull must not be null or blank.");
            }

            if (IsEncryptionEnabled())
            {
                data = crypto.UnEncryptIt(data);
            }

            return data;
        }

        /// <summary>
        /// Decrypt the encrypted configuration value.
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
        ///     <term>11/1/2016</term>
        ///     <term>Ruffa Martin</term>
        ///     <term>ClearText encryption project.</term>
        ///     <term>Initial Implementation</term>
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="value">The value that needs to be unencrpyted.</param>
        public static string Decrypt(string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim()))
            {
                return null;
            }

            if (IsEncryptionEnabled())
            {
                return crypto.UnEncryptIt(value);
            }

            return value;
        }

        /// <summary>
        /// Decrypts items if it is necessary
        /// </summary>
        /// <param name="encryptedDictionary">dictionary with encrypted items</param>        
        public static Dictionary<string, string> Decrypt(Dictionary<string, string> encryptedDictionary)
        {
            var decryptedDictionary = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> item in encryptedDictionary)
            {
                decryptedDictionary.Add(item.Key, Decrypt(item.Value));
            }

            return decryptedDictionary;
        }

        /// <summary>
        /// Unencrypt the data passed in. If able to, use the GetKeyData or GetEncryptedKeyData methods unless using a section specific object.
        /// it just retrive data that is encrypted, no matter if it is on the web config or not. Is not recomendable to use this method unles it
        /// is really necesary.
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
        public static string UnencryptKeyData(string encryptedData)
        {
            return crypto.UnEncryptIt(encryptedData);
        }

        /// <summary>
        /// Creates a new object of arbitrary type <typeparamref name="T"/> from section data in configuration file.
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
        /// <param name="sectionName">Name as defined in the configuration file. i.e: FNBM.Providers.FDRProvider.MQSeries</param>
        /// <param name="TSectionType">The type of object you are expecting in return. i.e: FNBM.Configuration.MQSeriesSectionData</param>
        [Obsolete("This method is deprecated, you must use Decrypt.")]
        public static T GetSection<T>(string sectionName, T TSectionType)
        {
            object o = null;

            try
            {
                o = ConfigurationManager.GetSection(sectionName);
            }
            catch (Exception)
            {
                throw;
            }

            if (o == null)
            {
                throw new ConfigurationErrorsException("Section: " + sectionName + " was not found in the configuration file.");
            }

            try
            {
                TSectionType = (T)Convert.ChangeType(o, typeof(T));
            }
            catch (Exception)
            {
                throw;
            }

            return TSectionType;
        }

        /// <summary>
        /// This method verify if encryption is enabled or not.
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
        private static bool IsEncryptionEnabled()
        {
            try
            {
                return EncryptedData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
