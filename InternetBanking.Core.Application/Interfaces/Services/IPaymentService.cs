using InternetBanking.Core.Application.Dtos.Deposit;
using InternetBanking.Core.Application.ViewModels.Payment;

namespace InternetBanking.Core.Application.Interfaces.Services
{
	public interface IPaymentService
    {
        Task<DepositResponse> MakeCashAdvance(CashAdvanceViewModel vm);
        Task<DepositResponse> InterAccountTransaction(AccountToAccountViewModel vm);
        Task<ExpresoPayViewModel> GetExpresoPayViewModelAsync(string clientId);
        Task<ExpresoPayViewModel> ExpresoPayValidationAsync(ExpresoPayViewModel vm);
		Task<ExpresoPayViewModel> ExpresoPayAsync(ExpresoPayViewModel vm);
		Task<CreditCardPayViewModel> GetCreditCardPayViewModelAsync(string clientId);
		Task<CreditCardPayViewModel> CreditCardPayAsync(CreditCardPayViewModel vm);
		Task<LoanPayViewModel> GetLoanPayViewModelAsync(string clientId);
		Task<LoanPayViewModel> LoanPayAsync(LoanPayViewModel vm);
        Task<BeneficiaryPayViewModel> GetBeneficiaryPayViewModelAsync(string clientId);
		Task<BeneficiaryPayViewModel> BeneficiaryPayValidationAsync(BeneficiaryPayViewModel vm);
		Task<BeneficiaryPayViewModel> BeneficiaryPayAsync(BeneficiaryPayViewModel vm);
		Task<CashAdvanceViewModel> GetCashAdvanceViewModelAsync(string clientId);
		Task<AccountToAccountViewModel> GetAccountToAccountViewModelAsync(string clientId);
	}
}