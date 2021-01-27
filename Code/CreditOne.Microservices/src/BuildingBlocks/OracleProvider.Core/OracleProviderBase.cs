using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

using CreditOne.Microservices.BuildingBlocks.Common.Configuration;

using Oracle.ManagedDataAccess.Client;

namespace CreditOne.Microservices.BuildingBlocks.OracleProvider.Core
{
    public abstract class OracleProviderBase
    {
        #region Public Delegates

        public delegate void ProcessReturnParametersDelegate(OracleParameterCollection parameters);

        #endregion

        #region Private Members

        private readonly DatabaseFunctions _databaseFunctions;
        private readonly DbConnectionStrings _dbConnectionStrings;

        #endregion

        #region Protected Properties

        protected string ConnectionString
        {
            get
            {
                return _dbConnectionStrings.ConnectionStrings[ConnectionStringKey];
            }
        }

        protected string ConnectionStringKey { get; set; }

        #endregion

        #region Constructors

        public OracleProviderBase(DatabaseFunctions databaseFunctions,
                                  DbConnectionStrings dbConnectionStrings)
        {
            _databaseFunctions = databaseFunctions;
            _dbConnectionStrings = dbConnectionStrings;
        }

        #endregion

        #region Gets Sync Methods

        public List<TDataObject> Get<TDataObject>(string commandText,
                                                  OracleParameter returnCursorParameter,
                                                  Func<SmartDataRecord, TDataObject> populateData)
        {
            return Get(ConnectionString, commandText, new List<OracleParameter>(), returnCursorParameter, populateData);
        }

        public List<TDataObject> Get<TDataObject>(string dbConnectionString,
                                                  string commandText,
                                                  OracleParameter returnCursorParameter,
                                                  Func<SmartDataRecord, TDataObject> populateData)
        {
            return Get(dbConnectionString, commandText, new List<OracleParameter>(), returnCursorParameter, populateData);
        }

        public List<TDataObject> Get<TDataObject>(string commandText,
                                                  OracleParameter criteria,
                                                  OracleParameter returnCursorParameter,
                                                  OracleParameter oracleParameter,
                                                  Func<SmartDataRecord, TDataObject> populateData)
        {
            List<OracleParameter> criteriaList = new List<OracleParameter>
            {
                criteria
            };

            return Get(ConnectionString, commandText, criteriaList, returnCursorParameter, populateData);
        }

        public List<TDataObject> Get<TDataObject>(string dbConnectionString,
                                                  string commandText,
                                                  OracleParameter criteria,
                                                  OracleParameter returnCursorParameter,
                                                  OracleParameter oracleParameter,
                                                  Func<SmartDataRecord, TDataObject> populateData)
        {
            List<OracleParameter> criteriaList = new List<OracleParameter>
            {
                criteria
            };

            return Get(dbConnectionString, commandText, criteriaList, returnCursorParameter, populateData);
        }

        public List<TDataObject> Get<TDataObject>(string commandText,
                                                  List<OracleParameter> criteria,
                                                  OracleParameter returnCursorParameter,
                                                  Func<SmartDataRecord, TDataObject> populateData)
        {
            return Get(ConnectionString, commandText, criteria, returnCursorParameter, populateData);
        }

        public List<TDataObject> Get<TDataObject>(string dbConnectionString,
                                                  string commandText,
                                                  List<OracleParameter> criteria,
                                                  OracleParameter returnCursorParameter,
                                                  Func<SmartDataRecord, TDataObject> populateData)
        {
            try
            {
                if (string.IsNullOrEmpty(dbConnectionString))
                {
                    throw new ArgumentNullException("dbConnectionString");
                }

                if (string.IsNullOrEmpty(commandText))
                {
                    throw new ArgumentNullException("commandText");
                }

                if (criteria == null)
                {
                    throw new ArgumentNullException("criteriaParameters");
                }

                if (returnCursorParameter == null)
                {
                    throw new ArgumentNullException("returnCursorParameter");
                }

                using (OracleConnection connection = new OracleConnection())
                {
                    using (OracleCommand command = _databaseFunctions.CreateCommand(connection, commandText, CommandType.StoredProcedure))
                    {
                        try
                        {
                            foreach (OracleParameter param in criteria)
                            {
                                command.Parameters.Add(param);
                            }

                            command.Parameters.Add(returnCursorParameter);

                            _databaseFunctions.OpenConnection(connection, dbConnectionString);

                            using (OracleDataReader reader = _databaseFunctions.ExecuteReader(command))
                            {
                                SmartDataRecord record = new SmartDataRecord(reader);

                                List<TDataObject> results = new List<TDataObject>();

                                while (_databaseFunctions.Read(reader, command))
                                {
                                    results.Add(populateData(record));
                                }

                                return results;
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new OracleProviderException("There was a problem while executing a database command", command, ex);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (criteria != null)
                {
                    foreach (OracleParameter parameter in criteria)
                    {
                        parameter.Dispose();
                    }
                }

                if (returnCursorParameter != null)
                {
                    returnCursorParameter.Dispose();
                    returnCursorParameter = null;
                }
            }
        }

        #endregion

        #region Gets Async Methods

        public async Task<List<TDataObject>> GetAsync<TDataObject>(string commandText,
                                                                   OracleParameter returnCursorParameter,
                                                                   Func<SmartDataRecord, TDataObject> populateData)
        {
            return await GetAsync(ConnectionString, commandText, new List<OracleParameter>(), returnCursorParameter, populateData);
        }

        public async Task<List<TDataObject>> GetAsync<TDataObject>(string dbConnectionString,
                                                                   string commandText,
                                                                   OracleParameter returnCursorParameter,
                                                                   Func<SmartDataRecord, TDataObject> populateData)
        {
            return await GetAsync(dbConnectionString, commandText, new List<OracleParameter>(), returnCursorParameter, populateData);
        }

        public async Task<List<TDataObject>> GetAsync<TDataObject>(string commandText,
                                                                   OracleParameter criteria,
                                                                   OracleParameter returnCursorParameter,
                                                                   OracleParameter oracleParameter,
                                                                   Func<SmartDataRecord, TDataObject> populateData)
        {
            return await GetAsync<TDataObject>(ConnectionString, commandText, criteria, returnCursorParameter, oracleParameter, populateData);
        }

        public async Task<List<TDataObject>> GetAsync<TDataObject>(string dbConnectionString,
                                                                   string commandText,
                                                                   OracleParameter criteria,
                                                                   OracleParameter returnCursorParameter,
                                                                   OracleParameter oracleParameter,
                                                                   Func<SmartDataRecord, TDataObject> populateData)
        {
            List<OracleParameter> criteriaList = new List<OracleParameter>
            {
                criteria
            };

            return await GetAsync(dbConnectionString, commandText, criteriaList, returnCursorParameter, populateData);
        }

        public async Task<List<TDataObject>> GetAsync<TDataObject>(string commandText,
                                                                   List<OracleParameter> criteria,
                                                                   OracleParameter returnCursorParameter,
                                                                   Func<SmartDataRecord, TDataObject> populateData)
        {
            return await GetAsync<TDataObject>(ConnectionString, commandText, criteria, returnCursorParameter, populateData);
        }

        public async Task<List<TDataObject>> GetAsync<TDataObject>(string dbConnectionString,
                                                                   string commandText,
                                                                   List<OracleParameter> criteria,
                                                                   OracleParameter returnCursorParameter,
                                                                   Func<SmartDataRecord, TDataObject> populateData)
        {
            try
            {
                if (string.IsNullOrEmpty(dbConnectionString))
                {
                    throw new ArgumentNullException("dbConnectionString");
                }

                if (string.IsNullOrEmpty(commandText))
                {
                    throw new ArgumentNullException("commandText");
                }

                if (criteria == null)
                {
                    throw new ArgumentNullException("criteriaParameters");
                }

                if (returnCursorParameter == null)
                {
                    throw new ArgumentNullException("returnCursorParameter");
                }

                using (OracleConnection connection = new OracleConnection())
                {
                    using (OracleCommand command = _databaseFunctions.CreateCommand(connection, commandText, CommandType.StoredProcedure))
                    {
                        try
                        {
                            foreach (OracleParameter param in criteria)
                            {
                                command.Parameters.Add(param);
                            }

                            command.Parameters.Add(returnCursorParameter);

                            await _databaseFunctions.OpenConnectionAsync(connection, dbConnectionString);

                            using (DbDataReader reader = await _databaseFunctions.ExecuteReaderAsync(command))
                            {
                                SmartDataRecord record = new SmartDataRecord(reader);

                                List<TDataObject> results = new List<TDataObject>();

                                while (await _databaseFunctions.ReadAsync(reader, command))
                                {
                                    results.Add(populateData(record));
                                }

                                return results;
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new OracleProviderException("There was a problem while executing a database command", command, ex);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (criteria != null)
                {
                    foreach (OracleParameter parameter in criteria)
                    {
                        parameter.Dispose();
                    }
                }

                if (returnCursorParameter != null)
                {
                    returnCursorParameter.Dispose();
                    returnCursorParameter = null;
                }
            }
        }

        #endregion

        #region Non-Queries Sync Methods

        protected void ExecuteNonQuery(string commandText)
        {
            ExecuteNonQuery(this.ConnectionString, commandText, null, null);
        }

        protected void ExecuteNonQuery(string dbConnectionString,
                                       string commandText)
        {
            ExecuteNonQuery(dbConnectionString, commandText, null, null);
        }

        protected void ExecuteNonQuery(string commandText,
                                       List<OracleParameter> parameters)
        {
            ExecuteNonQuery(this.ConnectionString, commandText, parameters, null);
        }

        protected void ExecuteNonQuery(string dbConnectionString,
                                       string commandText,
                                       List<OracleParameter> parameters)
        {
            ExecuteNonQuery(dbConnectionString, commandText, parameters, null);
        }

        protected void ExecuteNonQuery(string commandText,
                                       List<OracleParameter> parameters,
                                       ProcessReturnParametersDelegate processReturnParameters)
        {
            ExecuteNonQuery(this.ConnectionString, commandText, parameters, processReturnParameters);
        }

        protected void ExecuteNonQuery(string dbConnectionString,
                                       string commandText,
                                       List<OracleParameter> parameters,
                                       ProcessReturnParametersDelegate processReturnParameters)
        {
            try
            {
                if (string.IsNullOrEmpty(dbConnectionString))
                {
                    throw new ArgumentNullException("dbConnectionString");
                }

                if (string.IsNullOrEmpty(commandText))
                {
                    throw new ArgumentNullException("commandText");
                }

                using (OracleConnection connection = new OracleConnection())
                {
                    using (OracleCommand command = _databaseFunctions.CreateCommand(connection, commandText, CommandType.StoredProcedure, parameters))
                    {
                        try
                        {
                            _databaseFunctions.OpenConnection(connection, dbConnectionString);

                            _databaseFunctions.ExecuteNonQuery(command);

                            processReturnParameters?.Invoke(command.Parameters);
                        }
                        catch (Exception ex)
                        {
                            throw new OracleProviderException("There was a problem while executing a database command", command, ex);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (parameters != null)
                {
                    foreach (OracleParameter parameter in parameters)
                    {
                        parameter.Dispose();
                    }
                }
            }
        }

        #endregion

        #region Non-Queries Async Methods

        protected async Task<int> ExecuteNonQueryAsync(string dbConnectionString,
                                                       string commandText)
        {
            return await ExecuteNonQueryAsync(dbConnectionString, commandText, null, null);
        }

        protected async Task<int> ExecuteNonQueryAsync(string commandText)
        {
            return await ExecuteNonQueryAsync(this.ConnectionString, commandText, null, null);
        }

        protected async Task<int> ExecuteNonQueryAsync(string dbConnectionString,
                                                       string commandText,
                                                       List<OracleParameter> parameters)
        {
            return await ExecuteNonQueryAsync(dbConnectionString, commandText, parameters, null);
        }

        protected async Task<int> ExecuteNonQueryAsync(string commandText,
                                                       List<OracleParameter> parameters)
        {
            return await ExecuteNonQueryAsync(this.ConnectionString, commandText, parameters, null);
        }

        protected async Task<int> ExecuteNonQueryAsync(string commandText,
                                                       List<OracleParameter> parameters,
                                                       ProcessReturnParametersDelegate processReturnParameters)
        {
            return await ExecuteNonQueryAsync(this.ConnectionString, commandText, parameters, processReturnParameters);
        }

        protected async Task<int> ExecuteNonQueryAsync(string dbConnectionString,
                                                       string commandText,
                                                       List<OracleParameter> parameters,
                                                       ProcessReturnParametersDelegate processReturnParameters)
        {
            try
            {
                if (string.IsNullOrEmpty(dbConnectionString))
                {
                    throw new ArgumentNullException("dbConnectionString");
                }

                if (string.IsNullOrEmpty(commandText))
                {
                    throw new ArgumentNullException("commandText");
                }

                using (OracleConnection connection = new OracleConnection())
                {
                    using (OracleCommand command = _databaseFunctions.CreateCommand(connection, commandText, CommandType.StoredProcedure, parameters))
                    {
                        try
                        {
                            await _databaseFunctions.OpenConnectionAsync(connection, dbConnectionString);

                            var result = await _databaseFunctions.ExecuteNonQueryAsync(command);

                            processReturnParameters?.Invoke(command.Parameters);

                            return result;
                        }
                        catch (Exception ex)
                        {
                            throw new OracleProviderException("There was a problem while executing a database command", command, ex);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (parameters != null)
                {
                    foreach (OracleParameter parameter in parameters)
                    {
                        parameter.Dispose();
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Creates an OracleParameter object initializing the OracleDBTypeEx property
        /// </summary>
        /// <param name="name">the paramenter name</param>
        /// <param name="type">the parameter type</param>
        /// <param name="direction">the parameter direction</param>
        /// <returns>Returns an OracleParameter object with the OracleDbTypeEx property initialized.</returns>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>09/26/2014</term>
        ///		<term>Rafael Fernandez</term>
        ///		<term>WO59021</term>
        ///		<description>Initial Implementation</description>
        /// </item>
        /// </list>
        /// </remarks>
        protected OracleParameter CreateOracleParameter(string name,
                                                        OracleDbType type,
                                                        ParameterDirection direction)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            OracleParameter param = new OracleParameter(name, type, direction)
            {
                OracleDbTypeEx = type
            };

            return param;
        }

        /// <summary>
        /// Creates an OracleParameter object initializing the OracleDBTypeEx property
        /// </summary>
        /// <param name="name">the parameter name</param>
        /// <param name="type">the parameter type</param>
        /// <param name="value">the parameter value</param>
        /// <param name="direction">the parameter direction</param>
        /// <returns>Returns an OracleParameter object with the OracleDbTypeEx property initialized.</returns>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>09/26/2014</term>
        ///		<term>Rafael Fernandez</term>
        ///		<term>WO59021</term>
        ///		<description>
        ///		    Initial Implementation
        ///		</description>
        /// </item>
        /// </list>
        /// </remarks>
        protected OracleParameter CreateOracleParameter(string name,
                                                        OracleDbType type,
                                                        object value,
                                                        ParameterDirection direction)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            OracleParameter param = new OracleParameter(name, type, value, direction)
            {
                OracleDbTypeEx = type
            };

            return param;
        }

        /// <summary>
        /// Creates an OracleParameter object initializing the OracleDBTypeEx property
        /// </summary>
        /// <param name="name">the parameter name</param>
        /// <param name="type">the parameter type</param>
        /// <param name="size">the paramete size</param>
        /// <param name="value">the paramter value</param>
        /// <param name="direction">the parameter direction</param>
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
        ///		<term>10/02/2014</term>
        ///		<term>Rafael Fernandez</term>
        ///		<term>WO59021</term>
        ///		<description>
        ///		    Initial Implementation
        ///		</description>
        /// </item>
        /// </list>
        /// </remarks>
        protected OracleParameter CreateOracleParameter(string name,
                                                        OracleDbType type,
                                                        int size,
                                                        object value,
                                                        ParameterDirection direction)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            OracleParameter param = new OracleParameter(name, type, size, value, direction)
            {
                OracleDbTypeEx = type
            };

            return param;
        }
    }
}