using System;

namespace CreditOne.Microservices.BuildingBlocks.Logger.FileLogger
{
    /// <summary>
    /// Represent the log message struct.
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///  <term>Date</term>
    ///  <term>Who</term>
    ///  <term>BR/WO</term>
    ///  <description>Description</description>
    /// </listheader>
    /// <item>
    ///  <term>05-12-2020</term>
    ///  <term>Daniel Lobaton</term>
    ///  <term>WO376694</term>
    ///  <description>
    ///  Migrated from CreditOne.Operations.Common
    ///  </description>
    /// </item>
    /// </list>
    /// </remarks>
    public struct LogMessage
    {
        public DateTimeOffset Timestamp { get; set; }
        public string Message { get; set; }
    }
}