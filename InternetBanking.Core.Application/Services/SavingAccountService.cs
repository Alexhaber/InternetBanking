using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.SavingAccount;
using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    public class SavingAccountService : ISavingAccountService
    {
        private readonly SerialGenerator _serialGenerator;
        private readonly ISavingAccountRepository _savingAccountRepository;

        public SavingAccountService(SerialGenerator serialGenerator, ISavingAccountRepository savingAccountRepository)
        {
            _serialGenerator = serialGenerator;
            _savingAccountRepository = savingAccountRepository;
        }

        public async Task AddSavingAccountAsync(AddSavingAccountViewModel model)
        {
            try
            {

                var randomId = await _serialGenerator.GenerateSerial();
                if (string.IsNullOrEmpty(randomId))
                {
                    throw new InvalidOperationException("Error generating unique serial ID.");
                }
                if (string.IsNullOrEmpty(model.ClientId))
                {
                    throw new InvalidOperationException("Client ID cannot be null or empty.");
                }

                List<SavingAccount> cuentasUser = await _savingAccountRepository.GetAccountsByClientIdAsync(model.ClientId);
                if (cuentasUser == null)
                {
                    throw new InvalidOperationException("Error retrieving accounts for the specified client ID.");
                }


                var cantidadCuentas = cuentasUser.Count;
                SavingAccount cuenta = new()
                {
                    Id = randomId,
                    UserId = model.ClientId,
                    IsPrincipal = cantidadCuentas == 0,
                    Monto = model.Amount
                };


                await _savingAccountRepository.AddAsync(cuenta);
            }
            catch (InvalidOperationException ex)
            {

                throw new InvalidOperationException($"Operation failed: {ex.Message}", ex);
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while adding the saving account.", ex);
            }
        }
        public async Task AdminIncreaseMainSavingAccountAmmount(decimal amount, string userId)
        {
            List<SavingAccount> cuentasUser = await _savingAccountRepository.GetAccountsByClientIdAsync(userId);
            if (cuentasUser == null)
            {
                throw new InvalidOperationException($"Operation failed:");
            }
            foreach (var account in cuentasUser)
            {
                if (account.IsPrincipal && amount > 0)
                {
                    account.Monto += amount;
                    await _savingAccountRepository.UpdateAsync(account,account.Id);
                    break;
                }
            }
            
        }

    }

}