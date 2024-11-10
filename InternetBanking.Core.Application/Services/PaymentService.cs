using AutoMapper;
using InternetBanking.Core.Application.Dtos.Deposit;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Beneficiary;
using InternetBanking.Core.Application.ViewModels.CreditCard;
using InternetBanking.Core.Application.ViewModels.Loan;
using InternetBanking.Core.Application.ViewModels.Payment;
using InternetBanking.Core.Application.ViewModels.SavingAccount;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Core.Domain.Enums;
using System.ComponentModel;

namespace InternetBanking.Core.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly ISavingAccountRepository _savingAccountRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IBeneficiaryService _beneficiaryService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public PaymentService(ITransactionRepository transactionRepository,
                                ICreditCardRepository creditCardRepository,
                                ISavingAccountRepository savingAccountRepository,
                                ILoanRepository loanRepository,
                                IBeneficiaryService beneficiaryService,
                                IAccountService accountService,
                                IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _creditCardRepository = creditCardRepository;
            _savingAccountRepository = savingAccountRepository;
            _loanRepository = loanRepository;
            _beneficiaryService = beneficiaryService;
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<ExpresoPayViewModel> GetExpresoPayViewModelAsync(string clientId)
        {
            var accounts = await _savingAccountRepository.GetAccountsByClientIdAsync(clientId);
            return new ExpresoPayViewModel
            {
                Accounts = _mapper.Map<List<SavingAccountViewModel>>(accounts)
            };
        }

        public async Task<CreditCardPayViewModel> GetCreditCardPayViewModelAsync(string clientId)
        {
            var accounts = await _savingAccountRepository.GetAccountsByClientIdAsync(clientId);
            var creditCards = await _creditCardRepository.GetCreditCardsByClientIdAsync(clientId);

            return new CreditCardPayViewModel
            {
                Accounts = _mapper.Map<List<SavingAccountViewModel>>(accounts),
                CreditCards = _mapper.Map<List<CreditCardViewModel>>(creditCards)
            };
        }

        public async Task<LoanPayViewModel> GetLoanPayViewModelAsync(string clientId)
        {
            var accounts = await _savingAccountRepository.GetAccountsByClientIdAsync(clientId);
            var loans = await _loanRepository.GetLoansByClientIdAsync(clientId);

            return new LoanPayViewModel
            {
                Accounts = _mapper.Map<List<SavingAccountViewModel>>(accounts),
                Loans = _mapper.Map<List<LoanViewModel>>(loans)
            };
        }

        public async Task<BeneficiaryPayViewModel> GetBeneficiaryPayViewModelAsync(string clientId)
        {
            var accounts = await _savingAccountRepository.GetAccountsByClientIdAsync(clientId);
            var beneficiaries = await _beneficiaryService.GetBeneficiariesByClientId(clientId);

            return new BeneficiaryPayViewModel
            {
                Accounts = _mapper.Map<List<SavingAccountViewModel>>(accounts),
                Beneficiaries = _mapper.Map<List<BeneficiaryViewModel>>(beneficiaries)
            };
        }

        public async Task<CashAdvanceViewModel> GetCashAdvanceViewModelAsync(string clientId)
        {
            var accounts = await _savingAccountRepository.GetAccountsByClientIdAsync(clientId);
            var creditCards = await _creditCardRepository.GetCreditCardsByClientIdAsync(clientId);

            return new CashAdvanceViewModel
            {
                Accounts = _mapper.Map<List<SavingAccountViewModel>>(accounts),
                CreditCards = _mapper.Map<List<CreditCardViewModel>>(creditCards)
            };
        }

        public async Task<AccountToAccountViewModel> GetAccountToAccountViewModelAsync(string clientId)
        {
            var accounts = await _savingAccountRepository.GetAccountsByClientIdAsync(clientId);

            return new AccountToAccountViewModel
            {
                Accounts = _mapper.Map<List<SavingAccountViewModel>>(accounts)
            };
        }

        public async Task<BeneficiaryPayViewModel> BeneficiaryPayValidationAsync(BeneficiaryPayViewModel vm)
        {
            var account = await _savingAccountRepository.GetByIdAsync(vm.AccountId);

            if (account.Monto < vm.Monto)
            {
                vm.HasError = true;
                vm.Error = "Tu cuenta no tiene saldo suficiente para realizar el pago";
                return vm;
            }

            var beneficiaryAccount = await _savingAccountRepository.GetByIdAsync(vm.BeneficiaryAccountId);

            var ownerBeneficiaryAccount = await _accountService.GetUserById(beneficiaryAccount.UserId);

            vm.BeneficiaryName = $"{ownerBeneficiaryAccount.FirstName} {ownerBeneficiaryAccount.LastName}";

            return vm;
        }

        public async Task<BeneficiaryPayViewModel> BeneficiaryPayAsync(BeneficiaryPayViewModel vm)
        {
            var beneficiaryAccount = await _savingAccountRepository.GetByIdAsync(vm.BeneficiaryAccountId);
            var clientAccount = await _savingAccountRepository.GetByIdAsync(vm.AccountId);

            beneficiaryAccount.Monto += vm.Monto;
            clientAccount.Monto -= vm.Monto;

            var transaction = new Transaction
            {
                DestinationProductId = beneficiaryAccount.Id,
                SourceProductId = clientAccount.Id,
                Monto = vm.Monto
            };

            await _savingAccountRepository.UpdateAsync(beneficiaryAccount, beneficiaryAccount.Id);
            await _savingAccountRepository.UpdateAsync(clientAccount, clientAccount.Id);
            await _transactionRepository.AddAsync(transaction);

            vm.IsSucceeded = true;

            return vm;
        }

        public async Task<LoanPayViewModel> LoanPayAsync(LoanPayViewModel vm)
        {
            var loan = await _loanRepository.GetByIdAsync(vm.LoanId);
            var account = await _savingAccountRepository.GetByIdAsync(vm.AccountId);

            if (account.Monto < vm.Monto)
            {
                vm.HasError = true;
                vm.Error = "Tu cuenta no tiene saldo suficiente para realizar el pago";
                return vm;
            }

            decimal debt = loan.Monto - loan.Paid;
            decimal transactionAmount = vm.Monto;

            if (debt < transactionAmount)
            {
                transactionAmount = debt;
            }

            loan.Paid += transactionAmount;
            account.Monto -= transactionAmount;

            var transaction = new Transaction
            {
                DestinationProductId = loan.Id,
                SourceProductId = account.Id,
                Monto = transactionAmount
            };

            await _loanRepository.UpdateAsync(loan, loan.Id);
            await _savingAccountRepository.UpdateAsync(account, account.Id);
            await _transactionRepository.AddAsync(transaction);

            vm.IsSucceeded = true;
            return vm;
        }

        public async Task<CreditCardPayViewModel> CreditCardPayAsync(CreditCardPayViewModel vm)
        {
            var creditCard = await _creditCardRepository.GetByIdAsync(vm.CreditCardId);
            var account = await _savingAccountRepository.GetByIdAsync(vm.AccountId);

            if (account.Monto < vm.Monto)
            {
                vm.HasError = true;
                vm.Error = "Tu cuenta no tiene saldo suficiente para realizar el pago";
                return vm;
            }

            decimal debt = creditCard.Limit - creditCard.Monto;
            decimal transactionAmount = vm.Monto;

            if (debt < transactionAmount)
            {
                transactionAmount = debt;
            }

            creditCard.Monto += transactionAmount;
            account.Monto -= transactionAmount;

            var transaction = new Transaction
            {
                DestinationProductId = creditCard.Id,
                SourceProductId = account.Id,
                Monto = transactionAmount
            };

            await _creditCardRepository.UpdateAsync(creditCard, creditCard.Id);
            await _savingAccountRepository.UpdateAsync(account, account.Id);
            await _transactionRepository.AddAsync(transaction);

            vm.IsSucceeded = true;
            return vm;
        }

        public async Task<ExpresoPayViewModel> ExpresoPayValidationAsync(ExpresoPayViewModel vm)
        {
            var destinyAccount = await _savingAccountRepository.GetByIdAsync(vm.DestinyAccountId);

            if (destinyAccount == null)
            {
                vm.HasError = true;
                vm.Error = $"No existe cuenta con Id: {vm.DestinyAccountId}";
                return vm;
            }

            var originAccount = await _savingAccountRepository.GetByIdAsync(vm.OriginAccountId);

            if (originAccount.Monto < vm.Monto)
            {
                vm.HasError = true;
                vm.Error = "Tu cuenta no tiene saldo suficiente para realizar el pago";
                return vm;
            }

            var ownerDestinyAccount = await _accountService.GetUserById(destinyAccount.UserId);

            vm.DestinyAccountOwnerName = $"{ownerDestinyAccount.FirstName} {ownerDestinyAccount.LastName}";

            return vm;
        }

        public async Task<ExpresoPayViewModel> ExpresoPayAsync(ExpresoPayViewModel vm)
        {
            var destinyAccount = await _savingAccountRepository.GetByIdAsync(vm.DestinyAccountId);
            var originAccount = await _savingAccountRepository.GetByIdAsync(vm.OriginAccountId);

            destinyAccount.Monto += vm.Monto;
            originAccount.Monto -= vm.Monto;

            var transaction = new Transaction
            {
                DestinationProductId = destinyAccount.Id,
                SourceProductId = originAccount.Id,
                Monto = vm.Monto
            };

            await _savingAccountRepository.UpdateAsync(destinyAccount, destinyAccount.Id);
            await _savingAccountRepository.UpdateAsync(originAccount, originAccount.Id);
            await _transactionRepository.AddAsync(transaction);

            vm.IsSucceeded = true;

            return vm;
        }

        public async Task<DepositResponse> MakeCashAdvance(CashAdvanceViewModel vm)
        {
            var response = new DepositResponse();

            var FromCard = await _creditCardRepository.GetByIdAsync(vm.SenderProductId);

            if (FromCard == null)
            {
                response.HasError = true;
                response.Error = $"No se pudo encontrar la tarjeta de crédito";
                return response;
            }

            var ToAccount = await _savingAccountRepository.GetByIdAsync(vm.RecieverProductId);

            if (ToAccount == null)
            {
                response.HasError = true;
                response.Error = $"No se pudo encontrar la cuenta de ahorro";
                return response;
            }

            if (FromCard.Monto < vm.Monto)
            {
                response.HasError = true;
                response.Error = "El monto sobrepasa el limite de la tarjeta";

                return response;
            }

            ToAccount.Monto += vm.Monto;
            FromCard.Monto -= vm.Monto * 1.0625M;

            var transaction = new Transaction
            {
                SourceProductId = FromCard.Id,
                DestinationProductId = ToAccount.Id,
                Tipo = PaymentTypes.NormalTransaction,
                Monto = vm.Monto
            };

            await _savingAccountRepository.UpdateAsync(ToAccount, ToAccount.Id);
            await _creditCardRepository.UpdateAsync(FromCard, FromCard.Id);
            await _transactionRepository.AddAsync(transaction);

            return response;
        }

        public async Task<DepositResponse> InterAccountTransaction(AccountToAccountViewModel vm)
        {
            var response = new DepositResponse();

            var FromAccount = await _savingAccountRepository.GetByIdAsync(vm.SenderProductId);

            if (FromAccount == null)
            {
                response.HasError = true;
                response.Error = $"No se encontro la cuenta origen";
                return response;
            }

            var ToAccount = await _savingAccountRepository.GetByIdAsync(vm.RecieverProductId);

            if (ToAccount == null)
            {
                response.HasError = true;
                response.Error = $"No se encontro la cuenta destino";
                return response;
            }

            if (FromAccount.Monto < vm.Monto)
            {
                response.HasError = true;
                response.Error = $"El monto enviado sobrepasar el saldo actual de la cuenta";
                return response;
            }

            FromAccount.Monto -= vm.Monto;
            ToAccount.Monto += vm.Monto;

            var transaction = new Transaction
            {
                SourceProductId = vm.SenderProductId,
                DestinationProductId = vm.RecieverProductId,
                Tipo = PaymentTypes.NormalTransaction,
                Monto = vm.Monto
            };

            await _savingAccountRepository.UpdateAsync(ToAccount, ToAccount.Id);
            await _savingAccountRepository.UpdateAsync(FromAccount, FromAccount.Id);
            await _transactionRepository.AddAsync(transaction);

            return response;
        }
    }
}