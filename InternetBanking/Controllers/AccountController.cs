using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Controllers
{
	public class AccountController : Controller
	{
		private readonly IAccountService _accountService;

		public AccountController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		public IActionResult Login()
		{
			if (User.Identity.IsAuthenticated)
			{
				if (User.IsInRole("Admin"))
				{
					return RedirectToAction("Index", "Home"); //vista de admin
				}

				return RedirectToAction("Client", "Home"); //vista de cliente
			}

			return View(new LoginViewModel());
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel vm)
		{
			if(User.Identity.IsAuthenticated)
			{
				if(User.IsInRole("Admin"))
				{
					return RedirectToAction("Index", "Home"); //vista de admin
				}

				return RedirectToAction("Client", "Home"); //vista de cliente
			}

			if(!ModelState.IsValid)
			{
				return View(vm);
			}

			var response = await _accountService.AuthenticateAsync(vm);

			if(response.HasError)
			{
				return View(response);
			}

			if(response.Roles.Contains("Admin"))
			{
				return RedirectToAction("Index", "Home"); //vista de admin
			}

			return RedirectToAction("Client", "Home"); //vista de cliente
		}

		public async Task<IActionResult> LogOut()
		{
			await _accountService.SignOutAsync();
			return RedirectToAction("Login");
		}

		public IActionResult AccessDenied()
		{
			return View();
		}
	}
}
