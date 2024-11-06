using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.ViewModels.SavingAccount;
using InternetBanking.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface ISavingAccountService
    {
        Task AddSavingAccountAsync(AddSavingAccountViewModel model);
        Task AdminIncreaseMainSavingAccountAmmount(decimal amount, string userId);


    }
}