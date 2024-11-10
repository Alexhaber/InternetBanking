using InternetBanking.Core.Application.ViewModels.Account;
using InternetBanking.Core.Application.ViewModels.CreditCard;
using InternetBanking.Core.Application.ViewModels.Loan;
using InternetBanking.Core.Application.ViewModels.SavingAccount;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IAdminService
    {
        Task AddCreditCardAsync(AddCreditCardViewModel creditCard);
        Task AddLoanASync(AddLoanViewModel vm);
        Task AddSavingAccount(AddSavingAccountViewModel sAVm);
        Task DeleteCreditCardAsync(string id);
        Task DeleteLoanAsync(string id);
        Task DeleteSavingAccountAsync(string id, string clientId);
        Task ChangeUserState(string accountId);
        Task<AccountViewModel> GetAccountView(string accountId);
    }
}