using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.CreditCard;
using InternetBanking.Core.Application.ViewModels.Loan;
using InternetBanking.Core.Application.ViewModels.SavingAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Controllers
{
	[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> Productos(string id)
        {
            try
            {
                var profile = await _adminService.GetAccountView(id);
                if (profile != null)
                {
                    return View(profile);
                }
                ModelState.AddModelError(string.Empty, "El perfil no se encontró.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
            }

            return RedirectToAction("AdminIndex", "Home");
        }

        public async Task<IActionResult> ChangeUserStatus(string id)
        {
            await _adminService.ChangeUserState(id);
            return RedirectToAction("AdminIndex", "Home");
        }

        public IActionResult AddSavingAccount(string clientId)
        {
            return View(new AddSavingAccountViewModel { ClientId = clientId });
        }

        [HttpPost]
        public async Task<IActionResult> AddSavingAccount(AddSavingAccountViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                await _adminService.AddSavingAccount(model);
                return RedirectToAction("Productos", new { id = model.ClientId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return View(model);
            }
        }

        
        public IActionResult AddCreditCard(string clientId)
        {
            return View(new AddCreditCardViewModel { UserId = clientId });
        }

        [HttpPost]
        public async Task<IActionResult> AddCreditCard(AddCreditCardViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                await _adminService.AddCreditCardAsync(model);
                return RedirectToAction("Productos", new { id = model.UserId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return View(model);
            }
        }

        
        public IActionResult AddLoan(string clientId)
        {
            return View(new AddLoanViewModel { UserId = clientId });
        }

        [HttpPost]
        public async Task<IActionResult> AddLoan(AddLoanViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                await _adminService.AddLoanASync(model);
                return RedirectToAction("Productos", new { id = model.UserId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return View(model);
            }
        }

        
        [HttpPost]
        public async Task<IActionResult> DeleteSavingAccount(string id, string clientId)
        {
            try
            {
                await _adminService.DeleteSavingAccountAsync(id, clientId);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
            }
            return RedirectToAction("Productos");
        }

        
        [HttpPost]
        public async Task<IActionResult> DeleteCreditCard(string id)
        {
            try
            {
                await _adminService.DeleteCreditCardAsync(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
            }
            return RedirectToAction("Productos");
        }

        
        [HttpPost]
        public async Task<IActionResult> DeleteLoan(string id)
        {
            try
            {
                await _adminService.DeleteLoanAsync(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
            }
            return RedirectToAction("Productos");
        }
    }
}
