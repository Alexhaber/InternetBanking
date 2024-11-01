using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Beneficiary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InternetBanking.Controllers
{
	[Authorize(Roles = "Client")]
    public class BeneficiaryController : Controller
    {
        private readonly IBeneficiaryService _beneficiaryService;

        public BeneficiaryController(IBeneficiaryService beneficiaryService)
        {
            _beneficiaryService = beneficiaryService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _beneficiaryService.GetBeneficiariesByClientId(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        public IActionResult Create()
        {
            return View(new SaveBeneficiaryViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveBeneficiaryViewModel vm)
        {
            if(!ModelState.IsValid)
            {
                return View(vm);
            }

            await _beneficiaryService.Add(vm);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string ownerId, string beneficiaryId)
        {
            return View(await _beneficiaryService.GetBeneficiaryById(ownerId, beneficiaryId));
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(SaveBeneficiaryViewModel vm)
        {
            await _beneficiaryService.Remove(vm.UserId, vm.BeneficiaryAccountId);
            return RedirectToAction(nameof(Index)); 
        }
    }
}
