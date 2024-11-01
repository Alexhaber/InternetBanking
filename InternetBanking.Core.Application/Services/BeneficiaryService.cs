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

		public override async Task<SaveBeneficiaryViewModel> Add(SaveBeneficiaryViewModel vm)
		{
			var beneficiaryAccount = await _savingAccountRepository.GetByIdAsync(vm.BeneficiaryAccountId);

			if(beneficiaryAccount == null)
			{
				vm.HasError = true;
				vm.Error = $"The saving account with the id: {vm.BeneficiaryAccountId} doesn't exist";

				return vm;
			}

			if(await GetBeneficiaryById(vm.UserId, vm.BeneficiaryAccountId) != null)
			{
				vm.HasError = true;
				vm.Error = $"The saving account {vm.BeneficiaryAccountId} was already added as a beneficiary.You cannot add it again";

				return vm;
			}

			return await base.Add(vm);
		}

		public async Task<SaveBeneficiaryViewModel> GetBeneficiaryById(string userId, string beneficiaryId)
		{
			var beneficiary = await _beneficiaryRepository.GetBeneficiaryByIdAsync(userId, beneficiaryId);
			return _mapper.Map<SaveBeneficiaryViewModel>(beneficiary);
		}

		public async Task Remove(string userId, string beneficiaryId)
		{
			var beneficiary = await _beneficiaryRepository.GetBeneficiaryByIdAsync(userId, beneficiaryId);
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
