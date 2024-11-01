using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Interfaces.Repositories
{
	public interface ILoanRepository : IGenericRepository<Loan>
	{
		Task<List<Loan>> GetLoansByClientIdAsync(string clientId);
	}
}
