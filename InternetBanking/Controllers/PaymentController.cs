using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InternetBanking.Controllers
{
    [Authorize(Roles = "Client")]
    public class PaymentController : Controller
	{
		IPaymentService _paymentService;

		//////////puede ser termporal por se si se van a crear servicios/////////
		ICreditCardRepository _creditCardRepository;
		ISavingAccountRepository _savingAccountRepository;
		//___________________________________________________________________//

		IAccountService _accountService;

		public PaymentController(IPaymentService paymentService, IAccountService accountService, 
		ICreditCardRepository creditCardRepository, ISavingAccountRepository savingAccountRepository)
		{
			_paymentService = paymentService;
			_accountService = accountService;
			_creditCardRepository = creditCardRepository;
			_savingAccountRepository = savingAccountRepository;

		}

        [HttpGet]
		public async Task<IActionResult> CashAdvance()
		{
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var cardlist = await _creditCardRepository.GetCreditCardsByClientIdAsync(userId);
			var saveAccountList = await _savingAccountRepository.GetAccountsByClientIdAsync(userId);


            ViewBag.UserCards = cardlist;
			ViewBag.UserSaveAccounts = saveAccountList;

            return View();
		}

        [HttpPost]
        public async Task<IActionResult> CashAdvance(string CreditCardId, string SavingAccountId, decimal Amount)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var cardlist = await _creditCardRepository.GetCreditCardsByClientIdAsync(userId);
                var saveAccountList = await _savingAccountRepository.GetAccountsByClientIdAsync(userId);

                // Logica para procesar el pago
                var creditCard = await _creditCardRepository.GetByIdAsync(CreditCardId);
                var savingAccount = await _savingAccountRepository.GetByIdAsync(SavingAccountId);

                if (creditCard == null || savingAccount == null)
                {
                    ViewBag.UserCards = cardlist;
                    ViewBag.UserSaveAccounts = saveAccountList;
                    ModelState.AddModelError("", "Tarjeta de crédito o cuenta de ahorros no válida.");
                    return View(); // devuelve la vista con el error si algo no es válido.
                }

                // Implementar la lógica de pago usando el servicio de pago (_paymentService)
                var result = await _paymentService.MakeCashAdvance(creditCard, savingAccount, Amount);

                // determina la situacion del pago si no tiene error se manda al index, sino, se manda a la misma vista de cradit card
                if (!result.HasError)
                {

                    ViewBag.UserCards = cardlist;
                    ViewBag.UserSaveAccounts = saveAccountList;
                    TempData["SuccessMessage"] = "El pago fue procesado exitosamente.";
                    return View();
                }
                else
                {
                    

                    ViewBag.UserCards = cardlist;
                    ViewBag.UserSaveAccounts = saveAccountList;

                    TempData["ErrorMessage"] = result.Error;
                    return View();
                }
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> AccountToAccount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var saveAccountList = await _savingAccountRepository.GetAccountsByClientIdAsync(userId);

            ViewBag.UserSaveAccounts = saveAccountList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AccountToAccount(string FromAcoountId, string ToAccountId, decimal Amount)
        {
            try
            {
                //la misma vaina pero con 2 cuentas

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var saveAccountList = await _savingAccountRepository.GetAccountsByClientIdAsync(userId);
                var from = await _savingAccountRepository.GetByIdAsync(FromAcoountId);
                var to = await _savingAccountRepository.GetByIdAsync(ToAccountId);

                if (from == null || to == null)
                {
                    ViewBag.UserSaveAccounts = saveAccountList;
                    TempData["ErrorMessage"] = "Tarjeta de crédito o cuenta de ahorros no válida";
                    return View();
                }


                var result = await _paymentService.InterAccountTransaction(from, to, Amount);

                if (!result.HasError)
                {
                    ViewBag.UserSaveAccounts = saveAccountList;
                    TempData["SuccessMessage"] = "El pago fue procesado exitosamente";
                    return View();
                }
                else
                {

                    ViewBag.UserSaveAccounts = saveAccountList;
                    TempData["ErrorMessage"] = result.Error;
                    return View();
                }
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }
    }
}
