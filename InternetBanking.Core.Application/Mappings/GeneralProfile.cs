using AutoMapper;
using InternetBanking.Core.Application.ViewModels.Beneficiary;
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

			#region Beneficary
			CreateMap<Beneficiary, BeneficiaryViewModel>()
				.ForMember(vm => vm.BeneficiaryName, opt => opt.Ignore())
				.ReverseMap();

			CreateMap<Beneficiary, SaveBeneficiaryViewModel>()
				.ForMember(vm => vm.HasError, opt => opt.Ignore())
				.ForMember(vm => vm.Error, opt => opt.Ignore())
				.ReverseMap();
			#endregion
		}
	}
}
