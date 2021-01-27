using CreditOne.Microservices.BuildingBlocks.PerformanceMonitor.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;

namespace CreditOne.Microservices.BuildingBlocks.PerformanceMonitor.Database
{
    /// <summary>
    /// This implements a measurement interface which provides performance counters
    /// for the system to use.
    /// </summary>
    public class DatabaseMeasurement : MarshalByRefObject, IDatabaseMeasurement
    {
        #region Private Members

        private string _performanceCounterCategory;

        private PerformanceCounter _databaseConnectionOpenTime;
        private PerformanceCounter _databaseConnectionOpenTimeBase;

        private PerformanceCounter _databaseExecuteReaderTime;
        private PerformanceCounter _databaseExecuterReaderTimeBase;

        private PerformanceCounter _databaseExecuteNonQueryTime;
        private PerformanceCounter _databaseExecuteNonQueryTimeBase;

        private PerformanceCounter _databaseExecuteScalarTime;
        private PerformanceCounter _databaseExecuteScalarTimeBase;

        private PerformanceCounter _totalDatabaseOpenFailures;
        private PerformanceCounter _databaseOperationTimeThresholdExceededCount;

        #endregion

        #region Constructors

        public DatabaseMeasurement(IOptionsMonitor<PerformanceCounterCategoryConfiguration> options)
        {
            _performanceCounterCategory = options.CurrentValue.DatabaseCategory;
            CreateCounters();

            options.OnChange(OnConfigurationChange);
        }

        #endregion

        #region IMeasurement Implementation

        /// <summary>
        /// Counter for: Average Database Connection Open time in milliseconds
        /// </summary>
        public PerformanceCounter DatabaseConnectionOpenTime
        {
            get
            {
                return _databaseConnectionOpenTime;
            }
        }

        /// <summary>
        /// Counter for: Average Database Connection Open time in milliseconds Base
        /// </summary>
        public PerformanceCounter DatabaseConnectionOpenTimeBase
        {
            get
            {
                return _databaseConnectionOpenTimeBase;
            }
        }

        /// <summary>
        /// Counter for: Average Database ExecuteReader Time
        /// </summary>
        public PerformanceCounter DatabaseExecuteReaderTime
        {
            get
            {
                return _databaseExecuteReaderTime;
            }
        }

        /// <summary>
        /// Counter for: Average Database ExecuteReader Time Base
        /// </summary>
        public PerformanceCounter DatabaseExecuteReaderTimeBase
        {
            get
            {
                return _databaseExecuterReaderTimeBase;
            }
        }

        /// <summary>
        /// Counter for: Average Database ExecuteNonQuery Time
        /// </summary>
        public PerformanceCounter DatabaseExecuteNonQueryTime
        {
            get
            {
                return _databaseExecuteNonQueryTime;
            }
        }

        /// <summary>
        /// Counter for: Average Database ExecuteNonQuery Time Base
        /// </summary>
        public PerformanceCounter DatabaseExecuteNonQueryTimeBase
        {
            get
            {
                return _databaseExecuteNonQueryTimeBase;
            }
        }

        /// <summary>
        /// Counter for: Average Database ExecuteScalar Time
        /// </summary>
        public PerformanceCounter DatabaseExecuteScalarTime
        {
            get
            {
                return _databaseExecuteScalarTime;
            }
        }

        /// <summary>
        /// Counter for: Average Database ExecuteScalar Time Base
        /// </summary>
        public PerformanceCounter DatabaseExecuteScalarTimeBase
        {
            get
            {
                return _databaseExecuteScalarTimeBase;
            }
        }

        /// <summary>
        /// Counter for: Total Database Open Failures
        /// </summary>
        public PerformanceCounter TotalDatabaseOpenFailures
        {
            get
            {
                return _totalDatabaseOpenFailures;
            }
        }

        /// <summary>
        /// Counter for: Total Long Queries
        /// </summary>
        public PerformanceCounter DatabaseOperationTimeThresholdExceededCount
        {
            get
            {
                return _databaseOperationTimeThresholdExceededCount;
            }
        }

        #endregion

        #region Private Member Methods

        /// <summary>
        /// Creates the individual counters.
        /// </summary>
        /// <remarks>
        /// <list>
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>01/23/2007</term>
        ///		<term>rrodriguez</term>
        ///		<term>MK164</term>
        ///		<description>Implemented Offer coordinator counters.</description>
        /// </item>
        /// <item>
        ///		<term>03/17/2009</term>
        ///		<term>rrodriguez</term>
        ///		<term>Acct122</term>
        ///		<description>Implemented ORR coodinator counters.</description>
        /// </item>
        /// <item>
        ///		<term>11/30/2010</term>
        ///		<term>rrodriguez</term>
        ///		<term>IT140</term>
        ///		<description>created new counters: _totalDNCPrivacyOptoutCoordinatorsCreated; _DNCPrivacyOptoutCreatedPerSecond</description>
        ///	</item>
        ///	<item>
        ///		<term>12/05/2012</term>
        ///		<term>rrodriguez</term>
        ///		<term>Comp42</term>
        ///		<description>created new counters: 
        ///		<c>TotalEsignCoordinatorsCreated</c> and <c>+ESignCoordinatorsCreatedPerSecond</c>.</description>
        ///	</item>
        ///	<item>
        ///		<term>01/23/2013</term>
        ///		<term>rrodriguez</term>
        ///		<term>Risk199</term>
        ///		<description>Implemented AdverseAction counters.</description>
        /// </item>
        /// </list>
        /// </remarks>
        private void CreateCounters()
        {
            lock (this)
            {
                // Average Database Connection Open Time Metrics
                _databaseConnectionOpenTime = new PerformanceCounter(_performanceCounterCategory, "Average Database Connection Open Time In Milliseconds", false);
                _databaseConnectionOpenTime.RawValue = 0;

                _databaseConnectionOpenTimeBase = new PerformanceCounter(_performanceCounterCategory, "Average Database Connection Open Time In Milliseconds Base", false);
                _databaseConnectionOpenTimeBase.RawValue = 0;

                // Average Database ExecuteReader Time Metrics
                _databaseExecuteReaderTime = new PerformanceCounter(_performanceCounterCategory, "Average Database ExecuteReader Time In Milliseconds", false);
                _databaseExecuteReaderTime.RawValue = 0;

                _databaseExecuterReaderTimeBase = new PerformanceCounter(_performanceCounterCategory, "Average Database ExecuteReader Time In Milliseconds Base", false);
                _databaseExecuterReaderTimeBase.RawValue = 0;

                // Average Database ExecuteNonQuery Time Metrics
                _databaseExecuteNonQueryTime = new PerformanceCounter(_performanceCounterCategory, "Average Database ExecuteNonQuery Time In Milliseconds", false);
                _databaseExecuteNonQueryTime.RawValue = 0;

                _databaseExecuteNonQueryTimeBase = new PerformanceCounter(_performanceCounterCategory, "Average Database ExecuteNonQuery Time In Milliseconds Base", false);
                _databaseExecuteNonQueryTimeBase.RawValue = 0;

                // Average Database ExecuteScalar Time Metrics
                _databaseExecuteScalarTime = new PerformanceCounter(_performanceCounterCategory, "Average Database ExecuteScalar Time In Milliseconds", false);
                _databaseExecuteScalarTime.RawValue = 0;

                _databaseExecuteScalarTimeBase = new PerformanceCounter(_performanceCounterCategory, "Average Database ExecuteScalar Time In Milliseconds Base", false);
                _databaseExecuteScalarTimeBase.RawValue = 0;

                // Total Database Open Failures
                _totalDatabaseOpenFailures = new PerformanceCounter(_performanceCounterCategory, "Total Database Open Failures", false);
                _totalDatabaseOpenFailures.RawValue = 0;

                // Total Long Queries
                _databaseOperationTimeThresholdExceededCount = new PerformanceCounter(_performanceCounterCategory, "Database Operation Time Threshold Exceeded Count", false);
                _databaseOperationTimeThresholdExceededCount.RawValue = 0;
            }
        }

        private void OnConfigurationChange(PerformanceCounterCategoryConfiguration options)
        {
            _performanceCounterCategory = options.DatabaseCategory;
            try
            {
                CreateCounters();
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}