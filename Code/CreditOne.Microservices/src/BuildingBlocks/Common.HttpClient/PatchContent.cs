using System.Net.Http;
using System.Text;

using Newtonsoft.Json;

namespace CreditOne.Microservices.BuildingBlocks.Common.HttpClient
{
    /// <summary>
    /// PatchContent.cs
    /// PatchContent class
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
    ///  <term>07-12-2019</term>
    ///  <term>Mauro Allende</term>
    ///  <term>RM-47</term>
    ///  <description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks>
    public class PatchContent : StringContent
    {
        #region Constructor

        public PatchContent(object value)
            : base(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json-patch+json")
        {
        }

        #endregion
    }
}
