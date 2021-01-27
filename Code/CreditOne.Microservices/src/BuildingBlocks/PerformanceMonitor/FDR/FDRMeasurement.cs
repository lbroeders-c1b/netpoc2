using System;
using System.Diagnostics;

namespace CreditOne.Microservices.BuildingBlocks.PerformanceMonitor.FDR
{
    /// <summary>
    /// This implements a measurement interface which provides performance counters
    /// for the system to use.
    /// </summary>
    public class FDRMeasurement : MarshalByRefObject, IFDRMeasurement
    {
        #region Private Constants

        const string PERFORMANCE_COUNTER_CATEGORY = "FNBM Middleware";

        #endregion

        #region Private Members

        private PerformanceCounter _fdrQueryTimePerTransaction;
        private PerformanceCounter _fdrQueryTimePerTransactionBase;

        private PerformanceCounter _totalFDRTimeouts;

        private PerformanceCounter _totalFdrTransactionUnAvailableErrors;

        #endregion

        #region Constructors

        public FDRMeasurement()
        {
            CreateCounters();
        }

        #endregion

        #region IMeasurement Implementation

        /// <summary>
        /// Counter for: Total FDR Schedule Coordinators Created
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
        ///		<term>12/16/09</term>
        ///		<term>Riley White</term>
        ///		<term>Comp-23</term>
        ///		<description>Initial implementation</description>
        /// </item>
        /// </list>
        /// </remarks>
        public PerformanceCounter TotalFdrScheduleCoordinatorsCreated
        {
            get { return _totalFdrScheduleCoordinatorsCreated; }
        }

        /// <summary>
        /// Counter for: FDR Schedule Coordinators Created / Second
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
        ///		<term>12/16/09</term>
        ///		<term>Riley White</term>
        ///		<term>Comp-23</term>
        ///		<description>Initial implementation</description>
        /// </item>
        /// </list>
        /// </remarks>
        public PerformanceCounter FdrScheduleCoordinatorsCreatedPerSecond
        {
            get { return _fdrScheduleCoordinatorsCreatedPerSecond; }
        }

        /// <summary>
        /// Counter for: Average FDR Query Time in milliseconds
        /// </summary>
        public PerformanceCounter FDRQueryTimePerTransaction
        {
            get
            {
                return _fdrQueryTimePerTransaction;
            }
        }

        /// <summary>
        /// Counter for: Average FDR Query Time in Milliseconds Base
        /// </summary>
        public PerformanceCounter FDRQueryTimePerTransactionBase
        {
            get
            {
                return _fdrQueryTimePerTransactionBase;
            }
        }

        /// <summary>
        /// Counter for: Total FDR Timeouts
        /// </summary>
        public PerformanceCounter TotalFDRTimeouts
        {
            get
            {
                return _totalFDRTimeouts;
            }
        }

        private PerformanceCounter _totalFdrViewNotSupportedErrors;
        /// <summary>
        /// Counter for: Total FDR View Not Supported Errors
        /// </summary>
        public PerformanceCounter TotalFdrViewNotSupportedErrors
        {
            get
            {
                return _totalFdrViewNotSupportedErrors;
            }
        }

        private PerformanceCounter _totalFdrRequiredProgramNotLocatedErrors;
        /// <summary>
        /// Counter for: Total FDR Required Program Not Located Errors
        /// </summary>
        public PerformanceCounter TotalFdrRequiredProgramNotLocatedErrors
        {
            get
            {
                return _totalFdrRequiredProgramNotLocatedErrors;
            }
        }

        /// <summary>
        /// Counter for: Total FdrTransactionUnAvailableErrors
        /// Coordinators Created
        /// </summary>
        /// <list type="table">
        /// <listheader>
        /// <term>Date</term>
        /// <term>Who</term>
        /// <term>BR/WO</term>
        /// <description>Description</description>
        /// </listheader>
        /// <item>
        /// <term>08/24/2017</term>
        /// <term>Prabhakaran Munuswamy</term>
        /// <term>WO124206</term>
        /// <description>Initial implementation</description>
        /// </item>
        /// </list>
        public PerformanceCounter TotalFdrTransactionUnAvailableErrors
        {
            get { return _totalFdrTransactionUnAvailableErrors; }
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
                
                // FDR Schedule Coordinator Metrics
                _totalFdrScheduleCoordinatorsCreated = new PerformanceCounter(PERFORMANCE_COUNTER_CATEGORY, "Total FdrSchedule Coordinators Created", false);
                _totalFdrScheduleCoordinatorsCreated.RawValue = 0;

                _fdrScheduleCoordinatorsCreatedPerSecond = new PerformanceCounter(PERFORMANCE_COUNTER_CATEGORY, "FdrSchedule Coordinators Created/Second", false);
                _fdrScheduleCoordinatorsCreatedPerSecond.RawValue = 0;

                // Average FDR Transaction Time Metrics
                _fdrQueryTimePerTransaction = new PerformanceCounter(PERFORMANCE_COUNTER_CATEGORY, "Average FDR Query Time in Milliseconds", false);
                _fdrQueryTimePerTransaction.RawValue = 0;

                _fdrQueryTimePerTransactionBase = new PerformanceCounter(PERFORMANCE_COUNTER_CATEGORY, "Average FDR Query Time in Milliseconds Base", false);
                _fdrQueryTimePerTransactionBase.RawValue = 0;

                // Total FDR Timeouts
                _totalFDRTimeouts = new PerformanceCounter(PERFORMANCE_COUNTER_CATEGORY, "Total FDR Timeouts", false);
                _totalFDRTimeouts.RawValue = 0;

                _totalFdrViewNotSupportedErrors = new PerformanceCounter(PERFORMANCE_COUNTER_CATEGORY, "Total FDR View Not Supported Errors", false);
                _totalFdrViewNotSupportedErrors.RawValue = 0;

                _totalFdrRequiredProgramNotLocatedErrors = new PerformanceCounter(PERFORMANCE_COUNTER_CATEGORY, "Total FDR Required Program Not Located Errors", false);
                _totalFdrRequiredProgramNotLocatedErrors.RawValue = 0;

                _totalFdrTransactionUnAvailableErrors = new PerformanceCounter(PERFORMANCE_COUNTER_CATEGORY, "Total FDR transaction unavailable error", false);
                _totalFdrTransactionUnAvailableErrors.RawValue = 0;
            }
        }

        #endregion
    }
}