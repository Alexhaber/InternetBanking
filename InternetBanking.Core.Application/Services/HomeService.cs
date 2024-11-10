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
		private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountService _accountService;
        
        private readonly IMapper _mapper;

		public HomeService( IAccountService accountService, ISavingAccountRepository savingAccountRepository, 
							ICreditCardRepository creditCardRepository, 
							ILoanRepository loanRepository,
							IMapper mapper, ITransactionRepository transactionRepository)
		{
			_accountService = accountService;
			_savingAccountRepository = savingAccountRepository;
			_creditCardRepository = creditCardRepository;
			_loanRepository = loanRepository;
			_transactionRepository = transactionRepository;
			_mapper = mapper;
            

        }
        public async Task<DashBoardViewModel> GetDashBoardAsync()
        {

            var allTransactions = await _transactionRepository.GetAllAsync();
            var accounts = await _accountService.GetAllUsers();

            var allLoans = await _loanRepository.GetAllAsync();
            var allCreditCards = await _creditCardRepository.GetAllAsync();
            var allSavingAccounts = await _savingAccountRepository.GetAllAsync();

            var allProducts = allLoans.Count + allCreditCards.Count + allSavingAccounts.Count;

            
            var activeClients = accounts.Where(account => account.IsEmailConfirmed).ToList();
            var inactiveClients = accounts.Where(account => !account.IsEmailConfirmed).ToList();

            
            var today = DateTime.Today;
            var todayTransactions = allTransactions.Where(t => t.Made == today).ToList();
            
            

            DashBoardViewModel dashBoardViewModel = new DashBoardViewModel
            {
                
                TransactionsCount = allTransactions.Count,
                TodayPayments = todayTransactions.Count,

                ActiveClients = activeClients.Count,
                InactiveClientsCount = inactiveClients.Count,
               
                Products = allProducts
            };

            return dashBoardViewModel;
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
