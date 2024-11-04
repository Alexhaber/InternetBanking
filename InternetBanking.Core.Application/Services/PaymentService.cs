using AutoMapper;
using InternetBanking.Core.Application.Dtos.Deposit;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    public class PaymentService : IPaymentService
	{
		private readonly ITransactionRepository _transactionRepository;
		private readonly IMapper _mapper;

		public PaymentService(ITransactionRepository transactionRepository, IMapper mapper)
		{
			_transactionRepository = transactionRepository;
			_mapper = mapper;
		}

		public async Task<DepositResponse> MakeCashAdvance(CreditCard card, SavingAccount savingAccount, decimal deposit)
		{
			var reponse = new DepositResponse();

			reponse = await _transactionRepository.MakeCashAdvance(card, savingAccount, deposit);

			return reponse;
		}

		public async Task<DepositResponse> InterAccountTransaction(SavingAccount sender, SavingAccount receiver, decimal deposit)
		{
			var reponse = new DepositResponse();

			reponse = await _transactionRepository.InterAccountTransaction(sender, receiver, deposit);

			return reponse;
		}
	}
}
