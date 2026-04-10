using FinanceApp.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FinanceApp.Web.Data.Services
{
    public class ExpensesService : IExpensesService
    {
        private readonly AppDbContext _context;
        public ExpensesService(AppDbContext context)
        {
            _context = context;
        }
        public async Task Add(Expense expense)
        {
            _context.Expenses.Add(expense);
            var account = await _context.Accounts.FindAsync(expense.AccountId);  // Find the associated account

            if (account != null)
            {

                account.Balance -= expense.Amount;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Expense>> GetAll()
        {
            var expenses = await _context.Expenses.ToListAsync();
            return expenses;
        }
        public async Task<Expense?> GetById(int id)
        {
            return await _context.Expenses.FindAsync(id);
        }
        public async Task Update(Expense expense)
        {
            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
            }
        }
        public IQueryable<object> GetChartData()
        {
            var data = _context.Expenses
                              .GroupBy(e => e.Category)
                              .Select(g => new
                              {
                                  Category = g.Key,
                                  Total = g.Sum(e => e.Amount)
                              });
            return data;
        }
        public IQueryable GetMonthlyData()
        {
            return _context.Expenses
                .GroupBy(e => new { e.Date.Year, e.Date.Month })
                .OrderBy(g => g.Key.Year) // Sort by Year first
                .ThenBy(g => g.Key.Month) // Then by Month number (1, 2, 3...)
                .Select(g => new {
                    month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Key.Month),
                    total = g.Sum(e => e.Amount)
        });
        }

        public async Task<decimal> GetTotalBalance()
        {
            return await _context.Accounts.SumAsync(a => a.Balance);
        }

        public async Task<decimal> GetMonthlyExpenses()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            return await _context.Expenses
                .Where(e => e.Date.Month == currentMonth && e.Date.Year == currentYear)
                .SumAsync(e => e.Amount);
        }

        public async Task<decimal> GetTotalInvestment()
        {
            // This specifically filters expenses where the category is 'Investment'
            return await _context.Expenses
                .Where(e => e.Category == "Investment")
                .SumAsync(e => e.Amount);
        }

    }
}
