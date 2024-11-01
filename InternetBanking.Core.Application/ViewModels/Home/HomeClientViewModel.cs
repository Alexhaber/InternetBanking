using InternetBanking.Core.Application.ViewModels.CreditCard;
using InternetBanking.Core.Application.ViewModels.Loan;
using InternetBanking.Core.Application.ViewModels.SavingAccount;

namespace InternetBanking.Core.Application.ViewModels.Home
{
	public class HomeClientViewModel
	{
        public List<AccountViewModel> SavingAccounts { get; set; }
		public List<CreditCardViewModel> CreditCards { get; set; }
		public List<LoanViewModel> Loans { get; set; }
	}
}
