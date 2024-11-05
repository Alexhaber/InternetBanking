using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InternetBanking.Controllers
{
	[Authorize(Roles = "Client")]
    public class PaymentController : Controller
	{
		private readonly IPaymentService _paymentService;

		public PaymentController(IPaymentService paymentService)
		{
			_paymentService = paymentService;
		}

        public async Task<IActionResult> ExpresoPayment()
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _paymentService.GetExpresoPayViewModelAsync(clientId));
        }

        [HttpPost]
        public async Task<IActionResult> ExpresoPayment(ExpresoPayViewModel vm)
        {
            if(!ModelState.IsValid)
            {
                var viewModel = await _paymentService.GetExpresoPayViewModelAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
                vm.Accounts = viewModel.Accounts;
				return View(vm);
            }

            var response = await _paymentService.ExpresoPayValidationAsync(vm);

            if(response.HasError)
            {
				var viewModel = await _paymentService.GetExpresoPayViewModelAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
                response.Accounts = viewModel.Accounts;
				return View(response);
            }

            return View("ConfirmExpresoPay", response);
        }

		[HttpPost]
		public async Task<IActionResult> ExpresoPaymentPost(ExpresoPayViewModel vm)
		{
			var response = await _paymentService.ExpresoPayAsync(vm);

			return View("ConfirmExpresoPay", response);
		}

		public async Task<IActionResult> CreditCardPayment()
		{
			var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			return View(await _paymentService.GetCreditCardPayViewModelAsync(clientId));
		}

		[HttpPost]
		public async Task<IActionResult> CreditCardPayment(CreditCardPayViewModel vm)
		{
			if (!ModelState.IsValid)
			{
                var viewModel = await _paymentService.GetCreditCardPayViewModelAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
                vm.Accounts = viewModel.Accounts;
                vm.CreditCards = viewModel.CreditCards;
				return View(vm);
			}

			var response = await _paymentService.CreditCardPayAsync(vm);

            if(response.HasError)
            {
				var viewModel = await _paymentService.GetCreditCardPayViewModelAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
				response.Accounts = viewModel.Accounts;
				response.CreditCards = viewModel.CreditCards;
			}

			return View(response);
		}

		public async Task<IActionResult> LoanPayment()
		{
			var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			return View(await _paymentService.GetLoanPayViewModelAsync(clientId));
		}

		[HttpPost]
		public async Task<IActionResult> LoanPayment(LoanPayViewModel vm)
		{
			if (!ModelState.IsValid)
			{
				var viewModel = await _paymentService.GetLoanPayViewModelAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
				vm.Accounts = viewModel.Accounts;
				vm.Loans = viewModel.Loans;
				return View(vm);
			}

			var response = await _paymentService.LoanPayAsync(vm);

			if (response.HasError)
			{
				var viewModel = await _paymentService.GetLoanPayViewModelAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
				response.Accounts = viewModel.Accounts;
				response.Loans = viewModel.Loans;
			}

			return View(response);
		}

        public async Task<IActionResult> BeneficiaryPayment()
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _paymentService.GetBeneficiaryPayViewModelAsync(clientId));
        }

		[HttpPost]
		public async Task<IActionResult> BeneficiaryPayment(BeneficiaryPayViewModel vm)
		{
			if (!ModelState.IsValid)
			{
				var viewModel = await _paymentService.GetBeneficiaryPayViewModelAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
				vm.Accounts = viewModel.Accounts;
                vm.Beneficiaries = viewModel.Beneficiaries;
				return View(vm);
			}

			var response = await _paymentService.BeneficiaryPayValidationAsync(vm);

			if (response.HasError)
			{
				var viewModel = await _paymentService.GetBeneficiaryPayViewModelAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
				response.Accounts = viewModel.Accounts;
				vm.Beneficiaries = viewModel.Beneficiaries;
				return View(response);
			}

			return View("ConfirmBeneficiaryPay", response);
		}


		[HttpPost]
		public async Task<IActionResult> BeneficiaryPaymentPost(BeneficiaryPayViewModel vm)
		{
			var response = await _paymentService.BeneficiaryPayAsync(vm);

			return View("ConfirmBeneficiaryPay", response);
		}

		public async Task<IActionResult> CashAdvance()
		{
            return View(await _paymentService.GetCashAdvanceViewModelAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
		}

        [HttpPost]
        public async Task<IActionResult> CashAdvance(CashAdvanceViewModel vm)
        {
			if(!ModelState.IsValid)
			{
				var viewModel = await _paymentService.GetCashAdvanceViewModelAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
				vm.Accounts = viewModel.Accounts;
				vm.CreditCards = viewModel.CreditCards;
				return View(vm);
			}

			var result = await _paymentService.MakeCashAdvance(vm);

			if (result.HasError)
			{
				TempData["ErrorMessage"] = result.Error;
				var viewModel = await _paymentService.GetCashAdvanceViewModelAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
				vm.Accounts = viewModel.Accounts;
				vm.CreditCards = viewModel.CreditCards;
				return View(vm);
			}

			TempData["SuccessMessage"] = "El pago fue procesado exitosamente.";
			return View(vm);
		}

        public async Task<IActionResult> AccountToAccount()
        {
			return View(await _paymentService.GetAccountToAccountViewModelAsync(User.FindFirstValue(ClaimTypes.NameIdentifier))); ;
        }

        [HttpPost]
        public async Task<IActionResult> AccountToAccount(AccountToAccountViewModel vm)
        {
			if (!ModelState.IsValid)
			{
				var viewModel = await _paymentService.GetAccountToAccountViewModelAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
				vm.Accounts = viewModel.Accounts;
				return View(vm);
			}

			var result = await _paymentService.InterAccountTransaction(vm);

			if (result.HasError)
			{
				TempData["ErrorMessage"] = result.Error;
				var viewModel = await _paymentService.GetAccountToAccountViewModelAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
				vm.Accounts = viewModel.Accounts;
				return View(vm);
			}

			TempData["SuccessMessage"] = "El pago fue procesado exitosamente.";
			return View(vm);
		}
    }
}
