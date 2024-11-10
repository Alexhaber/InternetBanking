using InternetBanking.Core.Application.ViewModels.Loan;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Linq;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface ILoanService
    {
        Task<Loan> AddLoanAsync(AddLoanViewModel model);
        Task DeleteLoanAsync(LoanViewModel model);
    }
}
