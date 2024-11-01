using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Interfaces.Repositories
{
	public interface ISavingAccountRepository : IGenericRepository<SavingAccount>
	{
		Task<List<SavingAccount>> GetAccountsByClientIdAsync(string clientId);
	}
}
