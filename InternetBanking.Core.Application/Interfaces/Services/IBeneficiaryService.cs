using InternetBanking.Core.Application.ViewModels.Beneficiary;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Interfaces.Services
{
	public interface IBeneficiaryService : IGenericService<BeneficiaryViewModel, SaveBeneficiaryViewModel, Beneficiary>
	{
		Task<List<BeneficiaryViewModel>> GetBeneficiariesByClientId(string clientId);
		Task<SaveBeneficiaryViewModel> GetBeneficiaryById(string ownerId, string beneficiaryId);
		Task Remove(string ownerId, string beneficiaryId);
	}
}
