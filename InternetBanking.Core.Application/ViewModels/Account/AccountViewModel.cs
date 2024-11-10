using InternetBanking.Core.Application.ViewModels.CreditCard;
using InternetBanking.Core.Application.ViewModels.Loan;
using InternetBanking.Core.Application.ViewModels.SavingAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Account
{
    public class AccountViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Cedula { get; set; }
        public List<SavingAccountViewModel>? UserSavingAccounts { get; set; } = new List<SavingAccountViewModel>();
        public List<LoanViewModel>? UserLoans { get; set; } = new List<LoanViewModel>();
        public List<CreditCardViewModel> UserCreditCards { get; set; } = new List<CreditCardViewModel>();

        
    }
}
