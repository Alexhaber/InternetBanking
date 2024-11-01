using AutoMapper;
using InternetBanking.Core.Application.ViewModels.CreditCard;
using InternetBanking.Core.Application.ViewModels.Loan;
using InternetBanking.Core.Application.ViewModels.SavingAccount;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Mappings
{
	public class GeneralProfile : Profile
	{
        public GeneralProfile()
        {
			#region SavingAccount
			CreateMap<SavingAccount, AccountViewModel>()
				.ReverseMap();
			#endregion

			#region CreditCard
			CreateMap<CreditCard, CreditCardViewModel>()
				.ForMember(vm => vm.Debt, opt => opt.Ignore())
				.ReverseMap();
			#endregion

			#region Loans
			CreateMap<Loan, LoanViewModel>()
				.ForMember(vm => vm.Debt, opt => opt.Ignore())
				.ReverseMap();
			#endregion
		}
	}
}
