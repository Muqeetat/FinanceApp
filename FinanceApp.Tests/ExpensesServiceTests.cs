using FinanceApp.Web.Data.Services;
using FinanceApp.Web.Models;
using FinanceApp.Web.Data;
using Microsoft.EntityFrameworkCore;

public class ExpensesServiceTests
{
    private DbContextOptions<AppDbContext> GetDbOptions()
    {
        // Use an In-Memory database so tests are fast and don't touch your real SQL server
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task AddExpense_ShouldDeductFromAccountBalance()
    {
        // ARRANGE: Set up the scenario
        var options = GetDbOptions();
        using var context = new AppDbContext(options);

        var account = new Account { Id = 1, Name = "Test Account", Balance = 1000m };
        context.Accounts.Add(account);
        await context.SaveChangesAsync();

        var service = new ExpensesService(context);
        var expense = new Expense
        {
            Amount = 200m,
            AccountId = 1,
            Description = "Test",
            Date = DateTime.Now,
            Category = "Utilities"
        };

        // ACT: Run the actual logic
        await service.Add(expense);

        // ASSERT: Check if the result is what we expected
        var updatedAccount = await context.Accounts.FindAsync(1);
        Assert.NotNull(updatedAccount);
        Assert.Equal(800m, updatedAccount.Balance);
    }
}