using InternetBanking.Core.Application.ViewModels.CreditCard;
using System;
using System.Linq;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface ICreditCardService
    {
        Task AddCreditCardAsync(AddCreditCardViewModel model);
        Task DeleteCreditCardAsync(CreditCardViewModel model);
    }
}
