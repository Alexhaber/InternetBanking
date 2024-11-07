using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.CreditCard;
using InternetBanking.Core.Application.ViewModels.Home;
using InternetBanking.Core.Application.ViewModels.Loan;
using InternetBanking.Core.Application.ViewModels.SavingAccount;

namespace InternetBanking.Core.Application.Services
{
	public class HomeService : IHomeService
	{
		private readonly ISavingAccountRepository _savingAccountRepository;
		private readonly ICreditCardRepository _creditCardRepository;
		private readonly ILoanRepository _loanRepository;
		private readonly IMapper _mapper;

		public HomeService(ISavingAccountRepository savingAccountRepository, 
							ICreditCardRepository creditCardRepository, 
							ILoanRepository loanRepository,
							IMapper mapper)
		{
			_savingAccountRepository = savingAccountRepository;
			_creditCardRepository = creditCardRepository;
			_loanRepository = loanRepository;
			_mapper = mapper;
		}

		public async Task<HomeClientViewModel> GetProductsByClientIdAsync(string clientId)
		{
			HomeClientViewModel vm = new();

			var accounts = await _savingAccountRepository.GetAccountsByClientIdAsync(clientId);
			vm.SavingAccounts = _mapper.Map<List<SavingAccountViewModel>>(accounts);

			var crediCards = await _creditCardRepository.GetCreditCardsByClientIdAsync(clientId);
			vm.CreditCards = _mapper.Map<List<CreditCardViewModel>>(crediCards);

			var loans = await _loanRepository.GetLoansByClientIdAsync(clientId);
			vm.Loans = _mapper.Map<List<LoanViewModel>>(loans);

			return vm;
		}
	}
}
