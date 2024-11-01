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

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

		[Authorize(Roles = "Client")]
		public async Task<IActionResult> Client()
		{
			var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			return View(await _homeService.GetProductsByClientIdAsync(clientId));
		}

        public IActionResult Index()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
