namespace CreditOne.Microservices.BuildingBlocks.PerformanceMonitor.Database
{
    public class DatabaseTimeoutThresholdConfiguration
    {
        public int ExecuteReaderLogThreshold { get; set; }
        public int ExecuteNonQueryLogThreshold { get; set; }
        public int ExecuteScalarLogThreshold { get; set; }
        public int AdapterFillLogThreshold { get; set; }
        public int ReaderReadLogThreshold { get; set; }
    }
}
