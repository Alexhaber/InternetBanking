using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Interfaces.Repositories
{
	public interface IBeneficiaryRepository : IGenericRepository<Beneficiary>
	{
		Task<List<Beneficiary>> GetBeneficiariesByClientIdAsync(string clientId);
		Task<Beneficiary?> GetBeneficiaryByIdAsync(string userId, string beneficiaryId);
	}
}
