
namespace CreditOne.Microservices.BuildingBlocks.BearerTokenProvider.Interfaces
{
    public interface ITokenProvider
    {
        /// <summary>
        /// Gets the authentication token.
        /// </summary>
        /// <returns>The bearer token.</returns>
        string GetToken();
    }
}
