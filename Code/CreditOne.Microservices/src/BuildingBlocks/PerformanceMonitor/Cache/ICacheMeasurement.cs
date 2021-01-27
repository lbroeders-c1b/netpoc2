using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CreditOne.Microservices.BuildingBlocks.PerformanceMonitor.Cache
{
    /// <summary>
    /// Interface definition for the Measurement subsystem.
    /// </summary>
    public interface ICacheMeasurement
    {
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
    }
}
