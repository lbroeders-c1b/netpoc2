using System;
using System.Diagnostics;

namespace CreditOne.Microservices.BuildingBlocks.PerformanceMonitor.Cache
{
    /// <summary>
    /// This implements a measurement interface which provides performance counters
    /// for the system to use.
    /// </summary>
    public class CacheMeasurement : MarshalByRefObject, ICacheMeasurement
    {
        #region Private Constants

        const string PERFORMANCE_COUNTER_CATEGORY = "Cache Performance Counter";

        #endregion

        #region Private Members

        private PerformanceCounter _cachedDataProviderFetchTime;

        private PerformanceCounter _cachedDataProviderFetchTimeBase;

        private PerformanceCounter _totalItemsInCachedDataStore;

        private PerformanceCounter _cachedDataProviderExceptionsCount;

        #endregion

        #region Constructors

        public CacheMeasurement()
        {
            CreateCounters();
        }

        #endregion

        #region IMeasurement Implementation

        /// <summary>
        /// Counter for: Average CachedDataProvider Fetch Time
        /// </summary>
        public PerformanceCounter CachedDataProviderFetchTime
        {
            get
            {
                return _cachedDataProviderFetchTime;
            }
        }

        /// <summary>
        /// Counter for: Average CachedDataProvider Fetch Time Base
        /// </summary>
        public PerformanceCounter CachedDataProviderFetchTimeBase
        {
            get
            {
                return _cachedDataProviderFetchTimeBase;
            }
        }

        /// <summary>
        /// Counter for : Total exceptions occurred in Cached Data Provider
        /// </summary>
        public PerformanceCounter CachedDataProviderExceptionsCount
        {
            get
            {
                return _cachedDataProviderExceptionsCount;
            }
        }

        /// <summary>
        /// Counter for: Total Items in Cached Data Store
        /// </summary>
        public PerformanceCounter TotalItemsInCachedDataStore
        {
            get
            {
                return _totalItemsInCachedDataStore;
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
                // Average CachedDataProvider Execution Time Metrics
                _cachedDataProviderFetchTime = new PerformanceCounter(PERFORMANCE_COUNTER_CATEGORY, "Average CachedDataProvider Fetch Time In Milliseconds", false);
                _cachedDataProviderFetchTime.RawValue = 0;

                _cachedDataProviderFetchTimeBase = new PerformanceCounter(PERFORMANCE_COUNTER_CATEGORY, "Average CachedDataProvider Fetch Time In Milliseconds Base", false);
                _cachedDataProviderFetchTimeBase.RawValue = 0;

                _cachedDataProviderExceptionsCount = new PerformanceCounter(PERFORMANCE_COUNTER_CATEGORY, "CachedDataProvider Exceptions Count", false);
                _cachedDataProviderExceptionsCount.RawValue = 0;

                // Total Number of Items in Cached Data Store
                _totalItemsInCachedDataStore = new PerformanceCounter(PERFORMANCE_COUNTER_CATEGORY, "Total Items In Cached Data Store", false);
                _totalItemsInCachedDataStore.RawValue = 0;
            }
        }

        #endregion
    }
}