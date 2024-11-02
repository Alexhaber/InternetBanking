using InternetBanking.Core.Application.Dtos.Deposit;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Interfaces.Repositories
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<DepositResponse> MakeCashAdvance(CreditCard card, SavingAccount savingAccount, decimal deposit);
    }
}