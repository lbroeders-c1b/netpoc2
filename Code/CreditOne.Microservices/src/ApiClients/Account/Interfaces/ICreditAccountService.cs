using System.Collections.Generic;
using System.Threading.Tasks;

using CreditOne.Microservices.ApiClients.Account.Models.Response;

namespace CreditOne.Microservices.ApiClients.Account.Interfaces
{
    /// <summary>
    /// Defines the credit account service interface    
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///     <term>Date</term>
    ///     <term>Who</term>
    ///     <term>BR/WO</term>
    ///     <description>Description</description>
    /// </listheader>
    /// <item>
    ///     <term>2/18/2020</term>
    ///     <term>Luis Petitjean</term>
    ///     <term>RM-80</term>
    ///     <description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks>
    public interface ICreditAccountService
    {
        /// <summary>
        /// Retrieves a credit accounts for a given credit account identifier
        /// </summary>
        /// <param name="CreditAccountId">Credit account identifier</param>
        /// <returns>Returns the credit account data for <c>ResponseAccount.CreditAccount</c></returns>
        Task<CreditAccountResponse> GetAsync(decimal creditAccountId);

        /// <summary>
        /// Gets the credit account's primary cardholder resource
        /// </summary>
        /// <param name="creditAccountId">Credit account identifier</param>
        /// <returns>The <see cref="PrimaryCardholder"/></returns>
        Task<PrimaryCardholderResponse> GetPrimaryCardholderModelAsync(decimal creditAccountId);

        /// <summary>
        /// Retrieves a credit accounts for a given customer identifier
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>List of credit accounts</returns>
        Task<List<CreditAccountResponse>> GetByCustomerIdAsync(decimal customerId);

        /// <summary>
        /// Gets the credit account's cardholders resource
        /// </summary>
        /// <param name="creditAccountId">The credit account identifier</param>
        /// <returns>List of cardholders</returns>
        Task<List<CardholderBaseResponse>> GetCardholdersAsync(decimal creditAccountId);

        /// <summary>
        /// Retrieves the emails for a given credit account identifier
        /// </summary>
        /// <param name="creditAccountId">The credit account identifier</param>
        /// <returns>List of emails</returns>
        Task<List<CreditAccountEmailResponse>> GetEmailsAsync(decimal creditAccountId);
    }
}
