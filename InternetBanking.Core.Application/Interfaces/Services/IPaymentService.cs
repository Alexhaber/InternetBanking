using InternetBanking.Core.Application.Dtos.Deposit;
using InternetBanking.Core.Application.ViewModels.Payment;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<DepositResponse> MakeCashAdvance(CreditCard card, SavingAccount savingAccount, decimal deposit);
        Task<DepositResponse> InterAccountTransaction(SavingAccount sender, SavingAccount receiver, decimal deposit);
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
	}
}