using System;
using System.Collections.Generic;

namespace CreditOne.Microservices.BuildingBlocks.Common.Configuration
{
    /// <summary>
    /// Represents the configuration of URIs 
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
    public class ApiEndPointsConfiguration
    {
        /// <summary>
        /// Uri dictionary where key is the web api and the value is the uri configuration
        /// </summary>
        public Dictionary<string, UriBuilder> Uris { get; set; }
    }
}
