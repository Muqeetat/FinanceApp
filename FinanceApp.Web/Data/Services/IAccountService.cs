using FinanceApp.Web.Models;

namespace FinanceApp.Web.Data.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAllAccounts();
        Task AddAccount(Account account);
    }
}
