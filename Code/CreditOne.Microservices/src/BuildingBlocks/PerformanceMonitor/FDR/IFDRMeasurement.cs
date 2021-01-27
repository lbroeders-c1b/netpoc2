using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CreditOne.Microservices.BuildingBlocks.PerformanceMonitor.FDR
{
    /// <summary>
    /// Interface definition for the Measurement subsystem.
    /// </summary>
    public interface IFDRMeasurement
    {
        #region ACH Performance Counters

        /// <summary>
        /// Counter for: Average ACH Subsystem Execution Time
        /// </summary>
        PerformanceCounter ACHSubsystemExecuteTime { get; }

        /// <summary>
        /// Counter for: Average ACH Subsystem Execution Time Base
        /// </summary>
        PerformanceCounter ACHSubsystemExecuteTimeBase { get; }

        #endregion

        #region Cache Performance Counters

        /// <summary>
        /// Counter for: Average CachedDataProvider Fetch Time
        /// </summary>
        PerformanceCounter CachedDataProviderFetchTime { get; }

        /// <summary>
        /// Counter for: Average CachedDataProvider Fetch Time Base
        /// </summary>
        PerformanceCounter CachedDataProviderFetchTimeBase { get; }

        /// <summary>
        /// Counter for : Total exceptions occurred in Cached Data Provider
        /// </summary>
        PerformanceCounter CachedDataProviderExceptionsCount { get; }

        /// <summary>
        /// Counter for: Total Items in Cached Data Store
        /// </summary>
        PerformanceCounter TotalItemsInCachedDataStore { get; }

        #endregion

        #region FDR Performance Counters

        /// <summary>
        /// Counter for: Average FDR Query Time in milliseconds
        /// </summary>
        PerformanceCounter FDRQueryTimePerTransaction { get; }

        /// <summary>
        /// Counter for: Average FDR Query Time in Milliseconds Base
        /// </summary>
        PerformanceCounter FDRQueryTimePerTransactionBase { get; }

        /// <summary>
        /// Counter for: Total FDR Timeouts
        /// </summary>
        PerformanceCounter TotalFDRTimeouts { get; }

        /// <summary>
        /// Counter for: Total FDR View Not Supported Errors
        /// </summary>
        PerformanceCounter TotalFdrViewNotSupportedErrors { get; }

        /// <summary>
        /// Counter for: Total FDR Required Program Not Located Errors
        /// </summary>
        PerformanceCounter TotalFdrRequiredProgramNotLocatedErrors { get; }

        /// <summary>
        /// Counter for: Total Transaction Unavailable Errors
        /// </summary>
        PerformanceCounter TotalFdrTransactionUnAvailableErrors { get; }

        #endregion

        #region General Performance Counters

        /// <summary>
        /// Counter for: Total Warnings Issued
        /// </summary>
        PerformanceCounter TotalWarnings { get; }

        /// <summary>
        /// Counter for: Total Errors Issued
        /// </summary>
        PerformanceCounter TotalErrors { get; }

        #endregion

        #region Oracle Performance Counters

        /// <summary>
        /// Counter for: Average Oracle Connection Open time in milliseconds
        /// </summary>
        PerformanceCounter OracleConnectionOpenTime { get; }

        /// <summary>
        /// Counter for: Average Oracle Connection Open time in milliseconds Base
        /// </summary>
        PerformanceCounter OracleConnectionOpenTimeBase { get; }

        /// <summary>
        /// Counter for: Average Oracle ExecuteReader Time
        /// </summary>
        PerformanceCounter OracleExecuteReaderTime { get; }

        /// <summary>
        /// Counter for: Average Oracle ExecuteReader Time Base
        /// </summary>
        PerformanceCounter OracleExecuteReaderTimeBase { get; }

        /// <summary>
        /// Counter for: Average Oracle ExecuteNonQuery Time
        /// </summary>
        PerformanceCounter OracleExecuteNonQueryTime { get; }

        /// <summary>
        /// Counter for: Average Oracle ExecuteNonQuery Time Base
        /// </summary>
        PerformanceCounter OracleExecuteNonQueryTimeBase { get; }

        /// <summary>
        /// Counter for: Average Oracle ExecuteScalar Time
        /// </summary>
        PerformanceCounter OracleExecuteScalarTime { get; }

        /// <summary>
        /// Counter for: Average Oracle ExecuteScalar Time Base
        /// </summary>
        PerformanceCounter OracleExecuteScalarTimeBase { get; }

        /// <summary>
        /// Counter for: Total Database Open Failures
        /// </summary>
        PerformanceCounter TotalDatabaseOpenFailures { get; }

        /// <summary>
        /// Counter for: Total Long Queries
        /// </summary>
        PerformanceCounter DatabaseOperationTimeThresholdExceededCount { get; }

        #endregion

        /// <summary>
        /// Counter for: Total TotalAdverseActionCoordinatorsCreated
        /// Coordinators Created 
        /// </summary>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>01/23/2013</term>
        ///		<term>rrodriguez</term>
        ///		<term>Risk199</term>
        ///		<description>Initial implementation</description>
        /// </item>
        /// </list>
        /// </remarks>
        PerformanceCounter TotalAdverseActionCoordinatorsCreated { get; }
        /// <summary>
        /// Counter for: Total AdverseActionCoordinatorsCreated per second
        /// Coordinators Created 
        /// </summary>
        /// <list type="table">
        /// <listheader>
        ///		<term>Date</term>
        ///		<term>Who</term>
        ///		<term>BR/WO</term>
        ///		<description>Description</description>
        /// </listheader>
        /// <item>
        ///		<term>01/23/2013</term>
        ///		<term>rrodriguez</term>
        ///		<term>Risk199</term>
        ///		<description>Initial implementation</description>
        /// </item>
        /// </list>
        /// </remarks>
        PerformanceCounter AdverseActionCoordinatorsCreatedPerSecond { get; }
    }
}
