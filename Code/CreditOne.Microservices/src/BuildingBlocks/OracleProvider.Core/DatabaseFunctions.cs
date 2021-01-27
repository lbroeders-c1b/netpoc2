using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using CreditOne.Microservices.BuildingBlocks.PerformanceMonitor.Database;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Oracle.ManagedDataAccess.Client;

namespace CreditOne.Microservices.BuildingBlocks.OracleProvider.Core
{
    public class DatabaseFunctions
    {
        #region Constants

        private const int DATABASE_TIMEOUT_THRESHOLD = 5000;

        #endregion

        #region Private Members

        private DatabaseTimeoutThresholdConfiguration _thresholdConfig;
        private readonly IDatabaseMeasurement _measurement;
        private readonly ILogger _logger;

        [DllImport("Kernel32.dll")]
        public static extern bool QueryPerformanceCounter(out long counterValue);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long frequency);

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="measurement">Measurement</param>
        /// <param name="logger">Logger</param>
        /// <param name="thresholdConfig">Threshold</param>
        public DatabaseFunctions(IDatabaseMeasurement measurement, 
                                ILogger<DatabaseFunctions> logger, 
                                IOptionsMonitor<DatabaseTimeoutThresholdConfiguration> thresholdConfig)
        {
            _measurement = measurement;
            _logger = logger;
            _thresholdConfig = thresholdConfig.CurrentValue;

            thresholdConfig.OnChange(OnConfigurationChange);
        }

        #endregion

        #region Connection Methods

        /// <summary>
        /// Opens a connection using the connection string specified in an app setting.
        /// </summary>
        /// <param name="connection">Connection to open</param>
        /// <param name="connectionString">Connection string</param>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>9/15/2009</term>
        ///		<term>Riley White</term>
        ///		<term>Risk-152</term>
        ///		<description>Initial Method</description>
        /// </item>
        /// </list>
        /// </remarks>
        public void OpenConnection(OracleConnection connection, string connectionString)
        {
            if (connectionString == null || connectionString == string.Empty)
            {
                throw new Exception(string.Format(
                    "AppSetting [{0}] should contain a database connection string, but no value was found!",
                    connectionString));
            }

            connection.ConnectionString = connectionString;

            try
            {
                OpenConnection(connection);
            }
            catch (OracleException e)
            {
                throw new InvalidOperationException(string.Format(
                    "Unable to open a connection using the connection string specified in [{0}] AppSetting.",
                    connectionString), e);
            }
        }


        /// <summary>
        /// Really all this does is wrap the Open call so we can get some performance metrics.
        /// </summary>
        /// <param name="connection">The OracleConnection to open.</param>
        public void OpenConnection(OracleConnection connection)
        {
            // Record the start time
            long startTime;
            QueryPerformanceCounter(out startTime);
            try
            {
                // Open the connection
                connection.Open();
            }
            catch (OracleException)
            {
                _measurement.TotalDatabaseOpenFailures.Increment();
                throw;
            }
            finally
            {
                // Get the finish time
                long endTime;
                QueryPerformanceCounter(out endTime);

                // Update the performance counter. Multiply by 1000 to get value in milliseconds.
                _measurement.DatabaseConnectionOpenTime.IncrementBy((endTime - startTime) * 1000);
                _measurement.DatabaseConnectionOpenTimeBase.Increment();
            }
        }

        /// <summary>
        /// Opens a connection using the connection string specified in an app setting.
        /// </summary>
        /// <param name="connection">Connection to open</param>
        /// <param name="connectionString">Connection string</param>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>9/15/2009</term>
        ///		<term>Riley White</term>
        ///		<term>Risk-152</term>
        ///		<description>Initial Method</description>
        /// </item>
        /// </list>
        /// </remarks>
        public async Task OpenConnectionAsync(OracleConnection connection, string connectionString)
        {
            if (connectionString == null || connectionString == string.Empty)
            {
                throw new Exception(string.Format(
                    "AppSetting [{0}] should contain a database connection string, but no value was found!",
                    connectionString));
            }

            connection.ConnectionString = connectionString;

            try
            {
                await OpenConnectionAsync(connection);
            }
            catch (OracleException e)
            {
                throw new InvalidOperationException(string.Format(
                    "Unable to open a connection using the connection string specified in [{0}] AppSetting.",
                    connectionString), e);
            }
        }

        /// <summary>
        /// Really all this does is wrap the Open async call
        /// </summary>
        /// <param name="connection">The OracleConnection to open.</param>
        public async Task OpenConnectionAsync(OracleConnection connection)
        {
            // Record the start time
            long startTime;
            QueryPerformanceCounter(out startTime);

            try
            {
                // Open the connection
                await connection.OpenAsync();
            }
            catch (OracleException)
            {
                _measurement.TotalDatabaseOpenFailures.Increment();
                throw;
            }
            finally
            {
                // Get the finish time
                long endTime;
                QueryPerformanceCounter(out endTime);

                // Update the performance counter. Multiply by 1000 to get value in milliseconds.
                _measurement.DatabaseConnectionOpenTime.IncrementBy((endTime - startTime) * 1000);
                _measurement.DatabaseConnectionOpenTimeBase.Increment();
            }
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Formats the provided Oracle Command as string
        /// </summary>
        /// <param name="command">Oracle Command</param>
        /// <returns>Formated string (Oracle Command)</returns>
        private string FormatCommand(OracleCommand command)
        {
            if (command == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(command.CommandText);
            sb.AppendLine("(");

            foreach (OracleParameter parameter in command.Parameters)
            {
                sb.AppendFormat("\t{0} => {1}{2}",
                                parameter.ParameterName,
                                parameter.Value == null || parameter.Value == DBNull.Value ? "Null" : parameter.Value.ToString(),
                                Environment.NewLine);
            }

            sb.AppendLine(")");
            sb.AppendLine();

            return sb.ToString();
        }

        /// <summary>
        /// Creates an Oracle Command
        /// </summary>
        /// <param name="connection">Connection</param>
        /// <param name="commandText">Command Text</param>
        /// <param name="commandType">Command Type</param>
        /// <returns>Oracle Command</returns>
        internal OracleCommand CreateCommand(OracleConnection connection, string commandText, CommandType commandType)
        {
            return CreateCommand(connection, commandText, commandType, null);
        }

        /// <summary>
        /// Creates an Oracle Command
        /// </summary>
        /// <param name="connection">Connection</param>
        /// <param name="commandText">Command Text</param>
        /// <param name="commandType">Command Type</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Oracle Command</returns>
        internal OracleCommand CreateCommand(OracleConnection connection, string commandText, CommandType commandType, List<OracleParameter> parameters)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            if (string.IsNullOrEmpty(commandText))
                throw new ArgumentNullException("commandText");

            OracleCommand command = connection.CreateCommand();

            command.CommandText = commandText;
            command.CommandType = commandType;
            command.BindByName = true;

            if (parameters != null)
            {
                foreach (OracleParameter parameter in parameters)
                    command.Parameters.Add(parameter);
            }

            return command;
        }

        /// <summary>
        /// Invokes the ExecuteReader method on the passed OracleCommand object and returns the reader.
        /// </summary>
        /// <param name="command">A fully initialized OracleCommand object.</param>
        /// <returns>An OracleDataReader.</returns>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>06/03/2010</term>
        ///		<term>Riley White</term>
        ///		<term>MK-235</term>
        ///		<description>
        ///		New overload created with no methodName parameter. When logging is required,
        ///		we'll climb the stack trace to get the method name.
        ///		</description>
        /// </item>
        /// <item>
        ///		<term>07/20/2012</term>
        ///		<term>Riley White</term>
        ///		<term>RM-14 Phase 5</term>
        ///		<description>Removed methodName lookup. Let stack trace show method name, and let Log library find calling method.</description>
        /// </item>
        /// </list>
        /// </remarks>
        internal OracleDataReader ExecuteReader(OracleCommand command)
        {
            // Execute the reader. Record the start and end times.
            OracleDataReader reader;
            long startTime = 0;
            long endTime = 0;

            try
            {
                QueryPerformanceCounter(out startTime);
                reader = command.ExecuteReader();
                QueryPerformanceCounter(out endTime);

                return reader;
            }
            catch (OracleException)
            {
                // Close out the performance counter and throw the exception.
                QueryPerformanceCounter(out endTime);
                throw;
            }
            finally
            {
                // Update the performance counters
                _measurement.DatabaseExecuteReaderTime.IncrementBy((endTime - startTime) * 1000);
                _measurement.DatabaseExecuteReaderTimeBase.Increment();

                // Determine how much wall time this query took.
                long frequency;
                QueryPerformanceFrequency(out frequency);
                double milliseconds = ((double)(endTime - startTime) / (double)frequency) * 1000.00F;

                int loggingThreshold;
                if (_thresholdConfig.ExecuteReaderLogThreshold == 0)
                {
                    loggingThreshold = DATABASE_TIMEOUT_THRESHOLD;
                }
                else
                {
                    loggingThreshold = _thresholdConfig.ExecuteReaderLogThreshold;
                }

                if (milliseconds > loggingThreshold)
                {
                    _logger.LogWarning(string.Format("DatabaseFunctions - ExecuteReader - SLOW EXECUTE READER: {0:F3} milliseconds.{1}", milliseconds, FormatCommand(command)));

                    _measurement.DatabaseOperationTimeThresholdExceededCount.Increment();
                }
            }
        }

        /// <summary>
        /// Invokes the ExecuteReaderAsync method on the passed OracleCommand object and returns the reader.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Task<DbDataReader></returns>
        internal async Task<DbDataReader> ExecuteReaderAsync(OracleCommand command)
        {
            // Execute the reader. Record the start and end times.
            OracleDataReader reader;
            long startTime = 0;
            long endTime = 0;
            try
            {
                QueryPerformanceCounter(out startTime);
                reader = await command.ExecuteReaderAsync() as OracleDataReader;
                QueryPerformanceCounter(out endTime);
                return reader;
            }
            catch (OracleException)
            {
                // Close out the performance counter and throw the exception.
                QueryPerformanceCounter(out endTime);
                throw;
            }
            finally
            {
                // Update the performance counters
                _measurement.DatabaseExecuteReaderTime.IncrementBy((endTime - startTime) * 1000);
                _measurement.DatabaseExecuteReaderTimeBase.Increment();

                // Determine how much wall time this query took.
                long frequency;
                QueryPerformanceFrequency(out frequency);
                double milliseconds = ((double)(endTime - startTime) / (double)frequency) * 1000.00F;

                int loggingThreshold;
                if (_thresholdConfig.ExecuteReaderLogThreshold == 0)
                {
                    loggingThreshold = DATABASE_TIMEOUT_THRESHOLD;
                }
                else
                {
                    loggingThreshold = _thresholdConfig.ExecuteReaderLogThreshold;
                }

                if (milliseconds > loggingThreshold)
                {
                    _logger.LogWarning(string.Format("DatabaseFunctions - ExecuteReaderAsync - SLOW EXECUTE READER ASYNC: {0:F3} milliseconds.{1}", milliseconds, FormatCommand(command)));

                    _measurement.DatabaseOperationTimeThresholdExceededCount.Increment();
                }
            }
        }

        /// <summary>
        /// Invokes the ExecuteNonQuery method on the passed OracleCommand object.
        /// </summary>
        /// <param name="command">A fully initialized OracleCommand object.</param>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>06/03/2010</term>
        ///		<term>Riley White</term>
        ///		<term>MK-235</term>
        ///		<description>
        ///		New overload created with no methodName parameter. When logging is required,
        ///		we'll climb the stack trace to get the method name.
        ///		</description>
        /// </item>
        /// <item>
        ///		<term>07/20/2012</term>
        ///		<term>Riley White</term>
        ///		<term>RM-14 Phase 5</term>
        ///		<description>Removed methodName lookup. Let stack trace show method name, and let Log library find calling method.</description>
        /// </item>
        /// </list>
        /// </remarks>
        internal void ExecuteNonQuery(OracleCommand command)
        {
            // Execute the reader. Record the start and end times.
            long startTime = 0;
            long endTime = 0;
            try
            {
                QueryPerformanceCounter(out startTime);
                command.ExecuteNonQuery();
            }
            catch (OracleException)
            {
                // Close out the performance counter and throw the exception.
                QueryPerformanceCounter(out endTime);
                throw;
            }
            finally
            {
                QueryPerformanceCounter(out endTime);

                //// Update the performance counters
                _measurement.DatabaseExecuteNonQueryTime.IncrementBy((endTime - startTime) * 1000);
                _measurement.DatabaseExecuteNonQueryTime.Increment();

                //// Determine how much wall time this query took.
                long frequency;
                QueryPerformanceFrequency(out frequency);
                double milliseconds = ((double)(endTime - startTime) / (double)frequency) * 1000.00F;

                int loggingThreshold;
                if (_thresholdConfig.ExecuteNonQueryLogThreshold == 0)
                {
                    loggingThreshold = DATABASE_TIMEOUT_THRESHOLD;
                }
                else
                {
                    loggingThreshold = _thresholdConfig.ExecuteNonQueryLogThreshold;
                }

                if (milliseconds > loggingThreshold)
                {
                    _logger.LogWarning(string.Format("DatabaseFunctions - ExecuteNonQuery - SLOW EXECUTE NON-QUERY: {0:F3} milliseconds.{1}", milliseconds, FormatCommand(command)));

                    _measurement.DatabaseOperationTimeThresholdExceededCount.Increment();
                }
            }
        }

        /// <summary>
        /// Invokes the ExecuteNonQuery async method on the passed OracleCommand object.
        /// </summary>
        /// <param name="command">A fully initialized OracleCommand object.</param>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>06/03/2010</term>
        ///		<term>Riley White</term>
        ///		<term>MK-235</term>
        ///		<description>
        ///		New overload created with no methodName parameter. When logging is required,
        ///		we'll climb the stack trace to get the method name.
        ///		</description>
        /// </item>
        /// <item>
        ///		<term>07/20/2012</term>
        ///		<term>Riley White</term>
        ///		<term>RM-14 Phase 5</term>
        ///		<description>Removed methodName lookup. Let stack trace show method name, and let Log library find calling method.</description>
        /// </item>
        /// </list>
        /// </remarks>
        internal async Task<int> ExecuteNonQueryAsync(OracleCommand command)
        {
            // Execute the reader. Record the start and end times.
            long startTime = 0;
            long endTime = 0;
            try
            {
                QueryPerformanceCounter(out startTime);
                int result = await command.ExecuteNonQueryAsync();
                QueryPerformanceCounter(out endTime);
                return result;
            }
            catch (OracleException)
            {
                // Close out the performance counter and throw the exception.
                QueryPerformanceCounter(out endTime);
                throw;
            }
            finally
            {
                //// Update the performance counters
                _measurement.DatabaseExecuteNonQueryTime.IncrementBy((endTime - startTime) * 1000);
                _measurement.DatabaseExecuteNonQueryTime.Increment();

                //// Determine how much wall time this query took.
                long frequency;
                QueryPerformanceFrequency(out frequency);
                double milliseconds = ((double)(endTime - startTime) / (double)frequency) * 1000.00F;

                int loggingThreshold;
                if (_thresholdConfig.ExecuteNonQueryLogThreshold == 0)
                {
                    loggingThreshold = DATABASE_TIMEOUT_THRESHOLD;
                }
                else
                {
                    loggingThreshold = _thresholdConfig.ExecuteNonQueryLogThreshold;
                }

                if (milliseconds > loggingThreshold)
                {
                    _logger.LogWarning(string.Format("DatabaseFunctions - ExecuteNonQueryAsync - SLOW EXECUTE NON-QUERY: {0:F3} milliseconds.{1}", milliseconds, FormatCommand(command)));

                    _measurement.DatabaseOperationTimeThresholdExceededCount.Increment();
                }
            }
        }

        /// <summary>
        /// Invokes the ExecuteScalar method on the passed OracleCommand object.
        /// </summary>
        /// <param name="command">A fully initialized OracleCommand object.</param>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>06/03/2010</term>
        ///		<term>Riley White</term>
        ///		<term>MK-235</term>
        ///		<description>
        ///		New overload created with no methodName parameter. When logging is required,
        ///		we'll climb the stack trace to get the method name.
        ///		Also, we now returns the value from command.ExecuteScalar()...
        ///		</description>
        /// </item>
        /// <item>
        ///		<term>07/20/2012</term>
        ///		<term>Riley White</term>
        ///		<term>RM-14 Phase 5</term>
        ///		<description>Removed methodName lookup. Let stack trace show method name, and let Log library find calling method.</description>
        /// </item>
        /// </list>
        /// </remarks>
        internal object ExecuteScalar(OracleCommand command)
        {
            // Execute the reader. Record the start and end times.
            long startTime = 0;
            long endTime = 0;
            try
            {
                QueryPerformanceCounter(out startTime);
                object value = command.ExecuteScalar();
                QueryPerformanceCounter(out endTime);
                return value;
            }
            catch (OracleException)
            {
                // Close out the performance counter and throw the exception.
                QueryPerformanceCounter(out endTime);
                throw;
            }
            finally
            {
                // Update the performance counters
                _measurement.DatabaseExecuteScalarTime.IncrementBy((endTime - startTime) * 1000);
                _measurement.DatabaseExecuteScalarTime.Increment();

                // Determine how much wall time this query took.
                long frequency;
                QueryPerformanceFrequency(out frequency);
                double milliseconds = ((double)(endTime - startTime) / (double)frequency) * 1000.00F;

                int loggingThreshold;
                if (_thresholdConfig.ExecuteScalarLogThreshold == 0)
                {
                    loggingThreshold = DATABASE_TIMEOUT_THRESHOLD;
                }
                else
                {
                    loggingThreshold = _thresholdConfig.ExecuteScalarLogThreshold;
                }

                if (milliseconds > loggingThreshold)
                {
                    _logger.LogWarning(string.Format("DatabaseFunctions - ExecuteScalar - SLOW EXECUTE SCALAR: {0:F3} milliseconds.{1}", milliseconds, FormatCommand(command)));

                    _measurement.DatabaseOperationTimeThresholdExceededCount.Increment();
                }
            }
        }

        /// <summary>
        /// Invokes the ExecuteScalar async method on the passed OracleCommand object.
        /// </summary>
        /// <param name="command">A fully initialized OracleCommand object.</param>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>06/03/2010</term>
        ///		<term>Riley White</term>
        ///		<term>MK-235</term>
        ///		<description>
        ///		New overload created with no methodName parameter. When logging is required,
        ///		we'll climb the stack trace to get the method name.
        ///		Also, we now returns the value from command.ExecuteScalar()...
        ///		</description>
        /// </item>
        /// <item>
        ///		<term>07/20/2012</term>
        ///		<term>Riley White</term>
        ///		<term>RM-14 Phase 5</term>
        ///		<description>Removed methodName lookup. Let stack trace show method name, and let Log library find calling method.</description>
        /// </item>
        /// </list>
        /// </remarks>
        internal async Task<object> ExecuteScalarAsync(OracleCommand command)
        {
            // Execute the reader. Record the start and end times.
            long startTime = 0;
            long endTime = 0;
            try
            {
                QueryPerformanceCounter(out startTime);
                object value = await command.ExecuteScalarAsync();
                QueryPerformanceCounter(out endTime);
                return value;
            }
            catch (OracleException)
            {
                // Close out the performance counter and throw the exception.
                QueryPerformanceCounter(out endTime);
                throw;
            }
            finally
            {
                // Update the performance counters
                _measurement.DatabaseExecuteScalarTime.IncrementBy((endTime - startTime) * 1000);
                _measurement.DatabaseExecuteScalarTime.Increment();

                // Determine how much wall time this query took.
                long frequency;
                QueryPerformanceFrequency(out frequency);
                double milliseconds = ((double)(endTime - startTime) / (double)frequency) * 1000.00F;

                int loggingThreshold;
                if (_thresholdConfig.ExecuteScalarLogThreshold == 0)
                {
                    loggingThreshold = DATABASE_TIMEOUT_THRESHOLD;
                }
                else
                {
                    loggingThreshold = _thresholdConfig.ExecuteScalarLogThreshold;
                }

                if (milliseconds > loggingThreshold)
                {
                    _logger.LogWarning(string.Format("DatabaseFunctions - ExecuteScalarAsync - SLOW EXECUTE SCALAR: {0:F3} milliseconds.{1}", milliseconds, FormatCommand(command)));

                    _measurement.DatabaseOperationTimeThresholdExceededCount.Increment();
                }
            }
        }

        #endregion

        #region Reader/Record Methods

        /// <summary>
        /// Invokes the Read method on the passed OracleDataReader object.
        /// </summary>
        /// <param name="reader">A fully initialized OracleDataReader object.</param>
        /// <param name="command">A fully initialized OracleCommand object.</param>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>08/10/2012</term>
        ///		<term>Sherzod Niazov</term>
        ///		<term>IT-208</term>
        ///		<description>
        ///		Initial implementation.
        ///		</description>
        /// </item>
        /// </list>
        /// </remarks>
        internal bool Read(OracleDataReader reader, OracleCommand command)
        {
            // Execute the read meathod. Record the start and end times.
            long startTime = 0;
            long endTime = 0;

            bool readerResult = false;

            try
            {
                QueryPerformanceCounter(out startTime);
                readerResult = reader.Read();
            }
            catch (OracleException)
            {
                // Close out the performance counter and throw the exception.
                QueryPerformanceCounter(out endTime);
                throw;
            }
            finally
            {
                QueryPerformanceCounter(out endTime);

                // Update the performance counters
                _measurement.DatabaseExecuteNonQueryTime.IncrementBy((endTime - startTime) * 1000);
                _measurement.DatabaseExecuteNonQueryTime.Increment();

                // Determine how much wall time this query took.
                long frequency;
                QueryPerformanceFrequency(out frequency);
                double milliseconds = ((double)(endTime - startTime) / (double)frequency) * 1000.00F;

                int loggingThreshold;
                if (_thresholdConfig.ReaderReadLogThreshold == 0)
                {
                    loggingThreshold = DATABASE_TIMEOUT_THRESHOLD;
                }
                else
                {
                    loggingThreshold = _thresholdConfig.ReaderReadLogThreshold;
                }

                if (milliseconds > loggingThreshold)
                {
                    _logger.LogWarning(string.Format("DatabaseFunctions - Read - SLOW READ: {0:F3} milliseconds.{1}", milliseconds, FormatCommand(command)));

                    _measurement.DatabaseOperationTimeThresholdExceededCount.Increment();
                }
            }

            return readerResult;
        }

        /// <summary>
        /// Invokes the Read async method on the passed OracleDataReader object.
        /// </summary>
        /// <param name="reader">A fully initialized OracleDataReader object.</param>
        /// <param name="command">A fully initialized OracleCommand object.</param>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>08/10/2012</term>
        ///		<term>Sherzod Niazov</term>
        ///		<term>IT-208</term>
        ///		<description>
        ///		Initial implementation.
        ///		</description>
        /// </item>
        /// </list>
        /// </remarks>
        internal async Task<bool> ReadAsync(DbDataReader reader, OracleCommand command)
        {
            // Execute the read meathod. Record the start and end times.
            long startTime = 0;
            long endTime = 0;

            bool readerResult = false;

            try
            {
                QueryPerformanceCounter(out startTime);
                readerResult = await reader.ReadAsync();
                QueryPerformanceCounter(out endTime);
                return readerResult;
            }
            catch (OracleException)
            {
                // Close out the performance counter and throw the exception.
                QueryPerformanceCounter(out endTime);
                throw;
            }
            finally
            {
                // Update the performance counters
                _measurement.DatabaseExecuteNonQueryTime.IncrementBy((endTime - startTime) * 1000);
                _measurement.DatabaseExecuteNonQueryTime.Increment();

                // Determine how much wall time this query took.
                long frequency;
                QueryPerformanceFrequency(out frequency);
                double milliseconds = ((double)(endTime - startTime) / (double)frequency) * 1000.00F;

                int loggingThreshold;
                if (_thresholdConfig.ReaderReadLogThreshold == 0)
                {
                    loggingThreshold = DATABASE_TIMEOUT_THRESHOLD;
                }
                else
                {
                    loggingThreshold = _thresholdConfig.ReaderReadLogThreshold;
                }

                if (milliseconds > loggingThreshold)
                {
                    _logger.LogWarning(string.Format("DatabaseFunctions - ReadAsync - SLOW READ: {0:F3} milliseconds.{1}", milliseconds, FormatCommand(command)));

                    _measurement.DatabaseOperationTimeThresholdExceededCount.Increment();
                }
            }
        }

        #endregion

        #region Data Adapter Methods

        /// <summary>
        /// Invokes the Fill method on the passed dataadapter, filling the passed dataset.
        /// </summary>
        /// <param name="adapter">A fully initialized OracleDataAdapter object.</param>
        /// <param name="dataSet">The dataset to fill.</param>
        internal void Fill(OracleDataAdapter adapter, DataSet dataSet)
        {
            // Local variables
            long startTime = 0, endTime = 0;

            // Fill the dataset. Record the start and end times.
            try
            {
                QueryPerformanceCounter(out startTime);
                adapter.Fill(dataSet);
                QueryPerformanceCounter(out endTime);
            }
            catch (OracleException)
            {
                // Close out the performance counter and throw the exception.
                QueryPerformanceCounter(out endTime);
                throw;
            }
            finally
            {
                // Update the performance counters
                _measurement.DatabaseExecuteScalarTime.IncrementBy((endTime - startTime) * 1000);
                _measurement.DatabaseExecuteScalarTime.Increment();

                // Determine how much wall time this query took.
                long frequency;
                QueryPerformanceFrequency(out frequency);
                double milliseconds = ((double)(endTime - startTime) / (double)frequency) * 1000.00F;

                int loggingThreshold;
                if (_thresholdConfig.AdapterFillLogThreshold == 0)
                {
                    loggingThreshold = DATABASE_TIMEOUT_THRESHOLD;
                }
                else
                {
                    loggingThreshold = _thresholdConfig.AdapterFillLogThreshold;
                }

                if (milliseconds > loggingThreshold)
                {
                    _logger.LogWarning(string.Format("DatabaseFunctions - Fill - SLOW FILL: {0:F3} milliseconds.{1}", milliseconds, FormatCommand(adapter.SelectCommand)));

                    _measurement.DatabaseOperationTimeThresholdExceededCount.Increment();
                }
            }
        }

        #endregion

        #region Private Methods

        private void OnConfigurationChange(DatabaseTimeoutThresholdConfiguration options)
        {
            _thresholdConfig = options;
        }

        #endregion

    }
}
