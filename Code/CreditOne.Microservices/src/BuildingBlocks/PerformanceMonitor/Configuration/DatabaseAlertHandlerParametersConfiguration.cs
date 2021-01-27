namespace CreditOne.Microservices.BuildingBlocks.PerformanceMonitor.Configuration
{
    /// <summary>
    /// A wrapper class to contain configuration parameters
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///           <term>Date</term>
    ///           <term>Who</term>
    ///           <term>BR/WO</term>
    ///           <description>Description</description>
    /// </listheader>
    /// <item>
    ///           <term>6/12/2020</term>
    ///           <term>Juan Blanco</term>
    ///           <term>RM-79</term>
    ///           <term>Initial implementation</term>
    /// </item>
    /// </list>
    /// </remarks>

    public class DatabaseAlertHandlerParametersConfiguration
    {
        public AlertHandlerParameters DatabaseOpenFailureHandler { get; set; }
        public AlertHandlerParameters DatabaseOperationTimeThresholdHandler { get; set; }
    }

}
