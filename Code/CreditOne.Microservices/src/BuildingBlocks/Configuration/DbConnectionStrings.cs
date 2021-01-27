using System.Collections.Generic;

namespace CreditOne.Microservices.BuildingBlocks.Common.Configuration
{
    /// <summary>
    /// Represents the configuration of connection strings
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///     <term>Date</term>
    ///     <term>Who</term>
    ///     <term>BR/WO</term>
    ///     <term>Description</term>
    /// </listheader>
    /// <item>
    ///     <term>10/09/2019</term>
    ///     <term>Jonatan Marquez</term>
    ///     <term>RM-47</term>
    ///     <term>Initial Implementation</term>
    /// </item>
    /// </list>
    /// </remarks>
    public class DbConnectionStrings
    {
        /// <summary>
        /// Dictionary of connection strings where key is the schema and the value is the connection string
        /// </summary>
        public Dictionary<string, string> ConnectionStrings { get; set; }
    }
}
