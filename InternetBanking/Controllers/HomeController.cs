using InternetBanking.Core.Application.Dtos.User;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace InternetBanking.Controllers
{
	public class HomeController : Controller
	{
		private readonly IHomeService _homeService;
		private readonly IAccountService _accountService;

        public HomeController(IHomeService homeService, IAccountService accountService)
        {
            _homeService = homeService;
            _accountService = accountService;
        }

        [Authorize(Roles = "Client")]
		public async Task<IActionResult> Client()
		{
			var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			return View(await _homeService.GetProductsByClientIdAsync(clientId));
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AdminIndex()
		{
			var usuarios = await _accountService.GetAllUsers();            
			return View(usuarios);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Dashboard()
		{
			var dashboard =await _homeService.GetDashBoardAsync();
			return View(dashboard);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
