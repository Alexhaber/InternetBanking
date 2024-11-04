using AutoMapper;
using InternetBanking.Core.Application.Dtos.Deposit;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Services
{
    public class PaymentService : IPaymentService
	{
		private readonly ITransactionRepository _transactionRepository;
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly ISavingAccountRepository _savingAccountRepository;
		private readonly IMapper _mapper;

		public PaymentService(ITransactionRepository transactionRepository, 
                                ICreditCardRepository creditCardRepository, 
                                ISavingAccountRepository savingAccountRepository, 
                                IMapper mapper)
		{
			_transactionRepository = transactionRepository;
            _creditCardRepository = creditCardRepository;
            _savingAccountRepository = savingAccountRepository;
			_mapper = mapper;
		}

		public async Task<DepositResponse> MakeCashAdvance(CreditCard card, SavingAccount savingAccount, decimal deposit)
		{
			var response = new DepositResponse();

            var FromCard = await _creditCardRepository.GetByIdAsync(card.Id);

            if (FromCard == null)
            {
                response.HasError = true;
                response.Error = $"No se pudo encontrar la tarjeta de crédito";
                return response;
            }

            var ToAccount = await _savingAccountRepository.GetByIdAsync(savingAccount.Id);

            if (ToAccount == null)
            {
                response.HasError = true;
                response.Error = $"No se pudo encontrar la cuenta de ahorro";
                return response;
            }

            //si el saldo de la tarjeta recivida es menor que el monto recibidio devuelve la respuesta como erronea
            if (FromCard.Monto < deposit)
            {
                response.HasError = true;
                response.Error = "El monto sobrepasa el limite de la tarjeta";

                return response;
            }

            // se le suma el monto a la cuenta destino
            ToAccount.Monto += deposit;
            await _savingAccountRepository.UpdateAsync(ToAccount, ToAccount.Id);

            // se le quita el monto del deposito + impuesto(6.25%) a la tarjeta
            FromCard.Monto -= deposit * 1.0625M;
            await _creditCardRepository.UpdateAsync(FromCard, FromCard.Id);

            var transaction = new Transaction
            {
                SourceProductId = FromCard.Id,
                DestinationProductId = ToAccount.Id,
                Monto = deposit
            };

            await _transactionRepository.AddAsync(transaction);

            return response;
		}

		public async Task<DepositResponse> InterAccountTransaction(SavingAccount sender, SavingAccount receiver, decimal deposit)
		{
			var response = new DepositResponse();

            var FromAccount = await _savingAccountRepository.GetByIdAsync(sender.Id);

            if (FromAccount == null)
            {
                response.HasError = true;
                response.Error = $"No se encontro la cuenta origen";
                return response;
            }

            var ToAccount = await _savingAccountRepository.GetByIdAsync(receiver.Id);

            if (ToAccount == null)
            {
                response.HasError = true;
                response.Error = $"No se encontro la cuenta destino";
                return response;
            }

            if (FromAccount.Monto < deposit)
            {
                response.HasError = true;
                response.Error = $"El monto enviado sobrepasar el saldo actual de la cuenta";
                return response;
            }

            FromAccount.Monto -= deposit;
            ToAccount.Monto += deposit;

            await _savingAccountRepository.UpdateAsync(ToAccount, ToAccount.Id);
            await _savingAccountRepository.UpdateAsync(FromAccount, FromAccount.Id);

            var transaction = new Transaction
            {
                SourceProductId = sender.Id,
                DestinationProductId = receiver.Id,
                Monto = deposit
            };

            await _transactionRepository.AddAsync(transaction);

            return response;
		}
	}
}
