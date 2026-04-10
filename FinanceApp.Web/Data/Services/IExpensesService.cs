using FinanceApp.Web.Models;

namespace FinanceApp.Web.Data.Services
{
    public interface IExpensesService
    {
        Task<IEnumerable<Expense>> GetAll();
        Task<Expense?> GetById(int id);
        Task Add(Expense expense);
        Task Update(Expense expense);
        Task Delete(int id);
        IQueryable<object> GetChartData();
        IQueryable GetMonthlyData();
        Task<decimal> GetTotalBalance();
        Task<decimal> GetMonthlyExpenses();
        Task<decimal> GetTotalInvestment();
    }
}
