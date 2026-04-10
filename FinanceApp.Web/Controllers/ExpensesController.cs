using FinanceApp.Web.Data;
using FinanceApp.Web.Data.Services;
using FinanceApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Web.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly IExpensesService _expensesService;
        private readonly IAccountService _accountService;
        public ExpensesController(IExpensesService expensesService,IAccountService accountService)
        {
            _expensesService = expensesService;
            _accountService = accountService;
        }
        public async Task<IActionResult> Index()
        {
            var expenses = await _expensesService.GetAll();

            // Fetch the specific data for the three cards
            ViewBag.TotalBalance = await _expensesService.GetTotalBalance();
            ViewBag.MonthlyTotal = await _expensesService.GetMonthlyExpenses();
            ViewBag.TotalInvestment = await _expensesService.GetTotalInvestment();

            return View(expenses);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Accounts = await _accountService.GetAllAccounts();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expense expense)
        {
            if (ModelState.IsValid)
            {
                await _expensesService.Add(expense);

                return RedirectToAction("Index");
            }

            // If validation fails, we must reload the accounts for the dropdown
            ViewBag.Accounts = await _accountService.GetAllAccounts();
            return View(expense);
        }

        // GET: Expenses/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var expense = await _expensesService.GetById(id);
            if (expense == null) return NotFound();

            ViewBag.Accounts = await _accountService.GetAllAccounts();
            return View(expense);
        }

        // POST: Expenses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Expense expense)
        {
            if (id != expense.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                await _expensesService.Update(expense);
                return RedirectToAction("Index");
            }
            return View(expense);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _expensesService.Delete(id);
            return RedirectToAction("Index");
        }

        public IActionResult GetChart()
        {
            var data = _expensesService.GetChartData();

            //if the user hasn't added any expenses yet
            if (data == null || !data.Any())
            {
                return Json(new[] { new { Category = "No Data", Total = 0 } });
            }
            return Json(data);
        }

        [HttpGet]
        public IActionResult GetMonthlyData()
        {
            // Call the service method you just created
            var data = _expensesService.GetMonthlyData();

            // Return the data as JSON so Chart.js can read it
            return Json(data);
        }
    }
}
