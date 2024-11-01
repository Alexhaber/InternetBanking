using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Beneficiary;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Services
{
	public class BeneficiaryService : GenericService<BeneficiaryViewModel, SaveBeneficiaryViewModel, Beneficiary>, IBeneficiaryService
	{
		private readonly IBeneficiaryRepository _beneficiaryRepository;
		private readonly IAccountService _accountService;
		private readonly ISavingAccountRepository _savingAccountRepository;
		private readonly IMapper _mapper;

		public BeneficiaryService(IBeneficiaryRepository beneficiaryRepository, 
									IMapper mapper,
									IAccountService accountService,
									ISavingAccountRepository savingAccountRepository) : base(beneficiaryRepository, mapper)
		{
			_beneficiaryRepository = beneficiaryRepository;
			_mapper = mapper;
			_accountService = accountService;
			_savingAccountRepository = savingAccountRepository;
		}

		public async Task<SaveBeneficiaryViewModel> GetBeneficiaryById(string ownerId, string beneficiaryId)
		{
			var beneficiary = await _beneficiaryRepository.GetBeneficiaryByIdAsync(ownerId, beneficiaryId);
			return _mapper.Map<SaveBeneficiaryViewModel>(beneficiary);
		}

		public async Task Remove(string ownerId, string beneficiaryId)
		{
			var beneficiary = await _beneficiaryRepository.GetBeneficiaryByIdAsync(ownerId, beneficiaryId);
			await _beneficiaryRepository.DeleteAsync(beneficiary);
		}

		public async Task<List<BeneficiaryViewModel>> GetBeneficiariesByClientId(string clientId)
		{
			var beneficiaries = await _beneficiaryRepository.GetBeneficiariesByClientIdAsync(clientId);
			var beneficiarieViewModels = _mapper.Map<List<BeneficiaryViewModel>>(beneficiaries); 

			await AddBeneficiaryNames(beneficiarieViewModels);

			return beneficiarieViewModels;
		}

		private async Task AddBeneficiaryNames(List<BeneficiaryViewModel> beneficiaries)
		{
            foreach (var b in beneficiaries)
            {
				var beneficiaryAccount = await _savingAccountRepository.GetByIdAsync(b.BeneficiaryAccountId);
				var user = await _accountService.GetUserById(beneficiaryAccount.UserId);
				b.BeneficiaryName = $"{user.FirstName} {user.LastName}";
            }
        }
	}
}
