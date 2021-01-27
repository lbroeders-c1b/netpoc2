namespace CreditOne.Microservices.BuildingBlocks.Common.TokenProvider.Models
{
    /// <summary>
    /// Api Gateway Token Provider Configuration Class for Token Provider Building Block.
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
    ///  <term>06-15-2020</term>
    ///  <term>Favio Massarini</term>
    ///  <term>MK-1001</term>
    ///  <description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks>
    public class ApiGatewayTokenProviderConfiguration
    {
        public string AuthUri { get; set; }
        public string GrantType { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
