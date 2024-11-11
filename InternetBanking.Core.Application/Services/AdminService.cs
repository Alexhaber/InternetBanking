using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Account;
using InternetBanking.Core.Application.ViewModels.CreditCard;
using InternetBanking.Core.Application.ViewModels.Loan;
using InternetBanking.Core.Application.ViewModels.SavingAccount;

namespace InternetBanking.Core.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly ISavingAccountRepository _savingAccountRepository;
        private readonly ISavingAccountService _savingAccountService;
        private readonly ILoanRepository _loanRepository;
        private readonly ILoanService _loanService;
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly ICreditCardService _creditCardService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AdminService(ISavingAccountRepository savingAccountRepository, ISavingAccountService savingAccountService, ILoanRepository loanRepository, ILoanService loanService, ICreditCardRepository creditCardRepository, ICreditCardService creditCardService, IAccountService accountService, IMapper mapper)
        {
            _savingAccountRepository = savingAccountRepository;
            _savingAccountService = savingAccountService;
            _loanRepository = loanRepository;
            _loanService = loanService;
            _creditCardRepository = creditCardRepository;
            _creditCardService = creditCardService;
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task ChangeUserState(string accountId)
        {
            try
            {
                var user = await _accountService.GetUserById(accountId);
                await _accountService.ChangeUserState(user);
            }
            catch (Exception ex)
            {

            }
            
        }
        public async Task<AccountViewModel> GetAccountView(string accountId)
        {
            try
            {

                var user = await _accountService.GetUserById(accountId);
                if (user == null)
                {
                    throw new Exception($"User with ID '{accountId}' not .");
                }


                var userSavingAccounts = await _savingAccountRepository.GetAccountsByClientIdAsync(accountId);
                var userLoans = await _loanRepository.GetLoansByClientIdAsync(accountId);
                var userCreditCards = await _creditCardRepository.GetCreditCardsByClientIdAsync(accountId);


                AccountViewModel accountViewModel = _mapper.Map<AccountViewModel>(user);
                accountViewModel.UserSavingAccounts = _mapper.Map<List<SavingAccountViewModel>>(userSavingAccounts);
                accountViewModel.UserLoans = _mapper.Map<List<LoanViewModel>>(userLoans);
                accountViewModel.UserCreditCards = _mapper.Map<List<CreditCardViewModel>>(userCreditCards);



                return accountViewModel;
            }
            catch (AutoMapperMappingException ex)
            {

                throw new Exception("Error mapping data to AccountViewModel.", ex);
            }
            catch (ArgumentNullException ex)
            {

                throw new Exception("A required argument was null. Please ensure all required data is provided.", ex);
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while retrieving the account view.", ex);
            }
        }

        public async Task AddSavingAccount(AddSavingAccountViewModel sAVm)
        {
            try
            {

                var cliente = await _accountService.GetUserById(sAVm.ClientId);
                if (cliente == null)
                {
                    throw new Exception($"Client with ID '{sAVm.ClientId}' not found.");
                }


                await _savingAccountService.AddSavingAccountAsync(sAVm);
            }
            catch (ArgumentNullException ex)
            {

                throw new Exception("A required argument was null. Please ensure all required data is provided.", ex);
            }
            catch (InvalidOperationException ex)
            {

                throw new Exception("An invalid operation was attempted while adding the saving account.", ex);
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while adding the saving account.", ex);
            }
        }

        public async Task DeleteSavingAccountAsync(string id, string clientId)
        {
            var savingAccount = await _savingAccountRepository.GetByIdAsync(id);

            if (savingAccount == null)
            {
                throw new Exception("Account was not found.");
            }

            if (savingAccount.IsPrincipal)
            {
                throw new Exception("You can't delete your main saving account.");
            }

            if(savingAccount.Monto > 0)
            {
                var clientAccounts = await _savingAccountRepository.GetAccountsByClientIdAsync(clientId);
                var principalAccount = clientAccounts.FirstOrDefault(a => a.IsPrincipal);

                principalAccount.Monto += savingAccount.Monto;

                await _savingAccountRepository.UpdateAsync(principalAccount, principalAccount.Id);
            }

            await _savingAccountRepository.DeleteAsync(savingAccount);

        }

        public async Task AddCreditCardAsync(AddCreditCardViewModel creditCard)
        {
            await _creditCardService.AddCreditCardAsync(creditCard);
        }

        public async Task DeleteCreditCardAsync(string id)
        {
            var creditCard = await _creditCardRepository.GetByIdAsync(id);
            await _creditCardRepository.DeleteAsync(creditCard);
        }

        public async Task AddLoanASync(AddLoanViewModel vm)
        {
            var loan = await _loanService.AddLoanAsync(vm);
            var UserSavingAccount = await _savingAccountRepository.GetAccountsByClientIdAsync(vm.UserId);
            foreach (var account in UserSavingAccount) 
            {
                if (account.IsPrincipal)
                {
                    account.Monto += loan.Monto;
                    await _savingAccountRepository.UpdateAsync(account,account.Id);
                    break;
                }            
            }
        }

        public async Task DeleteLoanAsync(string id)
        {
            var loan = await _loanRepository.GetByIdAsync(id);
            await _loanRepository.DeleteAsync(loan);
        }
    }
}
