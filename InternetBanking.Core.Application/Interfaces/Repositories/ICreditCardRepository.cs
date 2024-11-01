using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Interfaces.Repositories
{
	public interface ICreditCardRepository : IGenericRepository<CreditCard>
	{
		Task<List<CreditCard>> GetCreditCardsByClientIdAsync(string clientId);
	}
}
