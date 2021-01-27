using System.Collections.Generic;
using System.Threading.Tasks;

using CreditOne.Microservices.Account.Models;

namespace CreditOne.Microservices.Account.Service
{
    public interface IAccountService
    {
        Task<(string inferedSearch, List<AccountModel> accountModels)> SearchBy(AccountSearchModel accountSearchModel);
    }
}
