using System.Threading.Tasks;

using CreditOne.Microservices.BuildingBlocks.Common.TokenProvider.Models;

namespace CreditOne.Microservices.BuildingBlocks.Common.TokenProvider.Interfaces
{
    /// <summary>
    /// Api Gateway Token Provider Interface for Token Provider Building Block.
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
    public interface IApiGatewayTokenProvider
    {
        /// <summary>
        /// Returns a new Token for Api Gateway request authentication.
        /// </summary>
        /// <returns>A <see cref="Token"/.></returns>
        Task<Token> GetToken();
    }
}