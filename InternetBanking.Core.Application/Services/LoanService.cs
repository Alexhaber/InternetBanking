using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Loan;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    public class LoanService : ILoanService
    {
        private readonly SerialGenerator _serialGenerator;
        private readonly ILoanRepository _loanRepository;

        public LoanService(SerialGenerator serialGenerator, ILoanRepository loanRepository)
        {
            _serialGenerator = serialGenerator;
            _loanRepository = loanRepository;
        }

        public async Task<Loan> AddLoanAsync(AddLoanViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "El modelo deL Prestamo  no puede ser nulo.");
            }



            if (string.IsNullOrEmpty(model.UserId))
            {
                throw new ArgumentException("El ID del usuario no puede estar vacío.", nameof(model.UserId));
            }

            var randomId = await _serialGenerator.GenerateSerial();
            Loan loan = new Loan(model.Monto)
            {
                Id = randomId,
                Paid = 0,
                UserId = model.UserId,
            };
            await _loanRepository.AddAsync(loan);
            return loan;
        }
        public async Task DeleteLoanAsync(LoanViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "El modelo deL Prestamo  no puede ser nulo.");
            }
            if (model.Debt > 0)
            {
                throw new InvalidOperationException("No se puede eliminar el prestamo porque tiene deuda pendiente.");
            }


            if (string.IsNullOrEmpty(model.UserId))
            {
                throw new ArgumentException("El ID del usuario no puede estar vacío.", nameof(model.UserId));
            }


            Loan loan = new Loan(model.Monto);
            loan.Paid = model.Paid;
            loan.UserId = model.UserId;

            await _loanRepository.DeleteAsync(loan);
        }
    }

}
