using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.CreditCard;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    public class CreditCardService : ICreditCardService
    {
        private readonly SerialGenerator _serialGenerator;
        private readonly ICreditCardRepository _creditCardRepository;

        public CreditCardService(SerialGenerator serialGenerator, ICreditCardRepository creditCardRepository)
        {
            _serialGenerator = serialGenerator;
            _creditCardRepository = creditCardRepository;
        }
        public async Task AddCreditCardAsync(AddCreditCardViewModel model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model), "El modelo de tarjeta de crédito no puede ser nulo.");
                }

                if (model.Limit <= 0)
                {
                    throw new ArgumentException("El límite de la tarjeta debe ser mayor a cero.", nameof(model.Limit));
                }
                if (model.Limit < model.Amount)
                {
                    throw new ArgumentException("Amount is higher than limit.", nameof(model.Limit));
                }
                if (string.IsNullOrEmpty(model.UserId))
                {
                    throw new ArgumentException("El ID del usuario no puede estar vacío.", nameof(model.UserId));
                }

                var randomId = await _serialGenerator.GenerateSerial();

                CreditCard card = new CreditCard(model.Limit)
                {
                    Id = randomId,
                    Limit = model.Limit,
                    Monto = model.Amount,
                    UserId = model.UserId,

                };

                await _creditCardRepository.AddAsync(card);
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException("Error al agregar la tarjeta de crédito.", ex);
            }

        }
        public async Task DeleteCreditCardAsync(CreditCardViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "El modelo de tarjeta de crédito no puede ser nulo.");
            }

            if (model.Debt > 0)
            {
                throw new InvalidOperationException("No se puede eliminar la tarjeta de crédito porque tiene deuda pendiente.");
            }

            var creditCard = await _creditCardRepository.GetByIdAsync(model.Id);
            if (creditCard == null)
            {
                throw new InvalidOperationException($"No se encontró una tarjeta de crédito con el ID '{model.Id}'.");
            }

            await _creditCardRepository.DeleteAsync(creditCard);
        }




    }
}
