using AutoMapper;
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
        private readonly IMapper _mapper;

        public SavingAccountService(SerialGenerator serialGenerator, ISavingAccountRepository savingAccountRepository, IMapper mapper)
        {
            _serialGenerator = serialGenerator;
            _savingAccountRepository = savingAccountRepository;
            _mapper = mapper;
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
                if (string.IsNullOrEmpty(model.UserId))
                {
                    throw new InvalidOperationException("Client ID cannot be null or empty.");
                }

                List<SavingAccount> cuentasUser = await _savingAccountRepository.GetAccountsByClientIdAsync(model.UserId);




                SavingAccount cuenta = _mapper.Map<SavingAccount>(model);
                cuenta.Id = randomId; 

                if (cuentasUser != null) 
                {
                    if (cuentasUser.Any()) {
                        cuenta.IsPrincipal = false;
                    }
                }

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