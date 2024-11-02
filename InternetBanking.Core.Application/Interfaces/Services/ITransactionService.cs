using InternetBanking.Core.Application.Dtos.Deposit;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface ITransactionService
    {
        Task<DepositResponse> MakeCashAdvance(CreditCard card, SavingAccount savingAccount, decimal deposit);

        Task<DepositResponse> InterAccountTransaction(SavingAccount sender, SavingAccount receiver, decimal deposit);

	}
}