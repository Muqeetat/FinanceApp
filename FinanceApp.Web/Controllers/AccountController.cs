using FinanceApp.Web.Data.Services;
using FinanceApp.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: Account/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Account account)
        {
            if (ModelState.IsValid)
            {
                await _accountService.AddAccount(account);
                return RedirectToAction("Index", "Expenses"); 
            }
            return View(account);
        }
    }
}
