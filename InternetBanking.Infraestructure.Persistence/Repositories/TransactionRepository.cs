using InternetBanking.Core.Application.Dtos.Deposit;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infraestructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using InternetBanking.Core.Application.Interfaces.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace InternetBanking.Infraestructure.Persistence.Repositories
{
    public class TransactionRepository : GenericRepository<InternetBanking.Core.Domain.Entities.Transaction>, ITransactionRepository
	{

		private readonly AppDbContext _context;

		public TransactionRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}

		///////////////////////////// Se puede poner el deposit request, estoy evaluando si poner el request o dejarlo asi///////////////////////////////////////////////
		public async Task<DepositResponse> MakeCashAdvance(CreditCard card, SavingAccount savingAccount, decimal deposit)
		{
			//Crea una respuesta de deposito e inicializa el error en falso 
			var response = new DepositResponse();
			response.HasError = false;

			var FromCard = await _context.CreditCards.FindAsync(card.Id);
			var ToAccount = await _context.SavingAccounts.FindAsync(savingAccount.Id);

			if (ToAccount == null)
			// si no se encuentra la tarjeta por x o y motivo se marca como erronea la solicitud y se le hace return
			{
				response.HasError = true;
				response.Error = $"No se pudo encontrar la tarjeta de crédito";
				return response;
			}
			else if (FromCard == null)
			//
			{
				response.HasError = true;
				response.Error = $"No se pudo encontrar la cuenta de ahorro";
				return response;
			}


			//si el limite de la tarjeta recivida es menor que el monto recibidio devuelve la respuesta como erronea
			if (FromCard.Limit < deposit)
			{
				response.HasError = true;
				response.Error = "El monto sobrepasa el limite de la tarjeta";

				return response;
			}

			// crea una transaccion toma como producto de origen el id de la tarjeta recibida, como el producto destinatario  la id de la cuenta 
			// recibida y como monto el deposito recibido
			var transaction = new InternetBanking.Core.Domain.Entities.Transaction
			{
				SourceProductId = card.Id,
				DestinationProductId = savingAccount.Id,
				Monto = deposit,
				Made = DateTime.Now,
			};

			try
			{
				//aparentemente esta es en la manera de agregar la transaccion y realizar los pagos sin guardar los datos de manera separada
				//porque sino, se corre el riesgo de que se agregue la transaccion sin que se agregue el pago o viceversa.
				using (var dbTransaction = await _context.Database.BeginTransactionAsync())
				{
					await _context.AddAsync(transaction);

					// monto con impuesto
					decimal totalToDeduct = deposit * 1.0625M;

					// se le resta el monto del deposito + impuesto a la tarjeta, recordemos que la deuda es: limite menos - monto
					FromCard.Monto -= totalToDeduct;

					//si se pasa de de 0 con el impuesto, entonces se fuerza a ser 0
					if (FromCard.Monto < 0)
					{
						FromCard.Monto = 0;
					}

					// se le suma el monto a la cuenta destino
					ToAccount.Monto += deposit;

					// se guardan los cambios del contexto
					await _context.SaveChangesAsync();

					// se hace commit de los cambios sucedidos dentro del la transaccion del 'using'
					await dbTransaction.CommitAsync();
				}

				return response;
			}
			catch (Exception ex)
			{
				//si ocurre un error en algun punto de la transaccion se devuelve este error
				response.HasError = true;
				response.Error = $"Ocurrio un error al realizar la transacción: {ex.Message}";
			}

			return response;
		}

		// escencialmente lo mismo que el otro metodo pero de cuenta a cuenta y sin impuesto
		public async Task<DepositResponse> InterAccountTransaction(SavingAccount sender, SavingAccount receiver, decimal deposit)
		{
			var response = new DepositResponse();
			response.HasError = false;

            try
            {
                var FromAccount = await _context.SavingAccounts.FindAsync(sender.Id);
				var ToAccount = await _context.SavingAccounts.FindAsync(receiver.Id);

				if (FromAccount == null)
				{
					response.HasError = true;
					response.Error = $"Sender account not found";
					//response.Error = $"Sender not found";
					return response;

				}
				else if (ToAccount == null)
				{ 
					response.HasError = true;
					response.Error = $"No se encontro la cuenta destino";
					return response;
				}

				if(sender.Monto < deposit)
				{
					response.HasError = true;
					response.Error = $"El monto enviado no puede sobrepasar su saldo actual";
					return response;
				}

				var transaction = new InternetBanking.Core.Domain.Entities.Transaction
				{
					SourceProductId = sender.Id,
					DestinationProductId = receiver.Id,
					Monto = deposit,
					Made = DateTime.Now,
				};

				using (var dbTransaction = await _context.Database.BeginTransactionAsync())
				{
					await _context.AddAsync(transaction);

					FromAccount.Monto -= deposit;
					ToAccount.Monto += deposit;

					await _context.SaveChangesAsync();
					await dbTransaction.CommitAsync();
				}

			}
			catch(Exception ex)
			{
				response.HasError = true;
				response.Error = $"Ocurrio un error: {ex.Message}";
			}

			return response;
		}
		/////////////////////////////////////////// fin de los cambios  ////////////////////////////////////////////////////////
	}
}
