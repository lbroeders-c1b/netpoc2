using System.Diagnostics;

namespace CreditOne.Microservices.BuildingBlocks.PerformanceMonitor.Database
{
    /// <summary>
    /// Interface definition for the Database Measurement subsystem.
    /// </summary>
    public interface IDatabaseMeasurement
    {
        /// <summary>
        /// Counter for: Average Database Connection Open time in milliseconds
        /// </summary>
        PerformanceCounter DatabaseConnectionOpenTime { get; }

        /// <summary>
        /// Counter for: Average Database Connection Open time in milliseconds Base
        /// </summary>
        PerformanceCounter DatabaseConnectionOpenTimeBase { get; }

        /// <summary>
        /// Counter for: Average Database ExecuteReader Time
        /// </summary>
        PerformanceCounter DatabaseExecuteReaderTime { get; }

        /// <summary>
        /// Counter for: Average Database ExecuteReader Time Base
        /// </summary>
        PerformanceCounter DatabaseExecuteReaderTimeBase { get; }

        /// <summary>
        /// Counter for: Average Database ExecuteNonQuery Time
        /// </summary>
        PerformanceCounter DatabaseExecuteNonQueryTime { get; }

        /// <summary>
        /// Counter for: Average Database ExecuteNonQuery Time Base
        /// </summary>
        PerformanceCounter DatabaseExecuteNonQueryTimeBase { get; }

        /// <summary>
        /// Counter for: Average Database ExecuteScalar Time
        /// </summary>
        PerformanceCounter DatabaseExecuteScalarTime { get; }

        /// <summary>
        /// Counter for: Average Database ExecuteScalar Time Base
        /// </summary>
        PerformanceCounter DatabaseExecuteScalarTimeBase { get; }

        /// <summary>
        /// Counter for: Total Database Open Failures
        /// </summary>
        PerformanceCounter TotalDatabaseOpenFailures { get; }

        /// <summary>
        /// Counter for: Total Long Queries
        /// </summary>
        PerformanceCounter DatabaseOperationTimeThresholdExceededCount { get; }
    }
}
