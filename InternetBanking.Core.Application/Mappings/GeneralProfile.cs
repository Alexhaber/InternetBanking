using AutoMapper;
using InternetBanking.Core.Application.Dtos.User;
using InternetBanking.Core.Application.ViewModels.Account;
using InternetBanking.Core.Application.ViewModels.Beneficiary;
using InternetBanking.Core.Application.ViewModels.CreditCard;
using InternetBanking.Core.Application.ViewModels.Loan;
using InternetBanking.Core.Application.ViewModels.Payment;
using InternetBanking.Core.Application.ViewModels.SavingAccount;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Core.Domain.Enums;

namespace InternetBanking.Core.Application.Mappings
{
	public class GeneralProfile : Profile
	{
        public GeneralProfile()
        {
            #region AddSavingAccount Mapping
            CreateMap<AddSavingAccountViewModel, SavingAccount>()
                .ForMember(dest => dest.Monto, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.IsPrincipal, opt => opt.MapFrom(src => true)) 
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            #endregion
            #region AccountToAccount Mapping
            CreateMap<AccountToAccountViewModel, Transaction>()
                .ForMember(dest => dest.SourceProductId, opt => opt.MapFrom(src => src.SenderProductId))
                .ForMember(dest => dest.DestinationProductId, opt => opt.MapFrom(src => src.RecieverProductId))
                .ForMember(dest => dest.Monto, opt => opt.MapFrom(src => src.Monto))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => PaymentTypes.NormalTransaction)) 
                .ForMember(dest => dest.Made, opt => opt.Ignore()) 
                .ForMember(dest => dest.Id, opt => opt.Ignore()); 
            #endregion
            #region BeneficiaryPay to Transaction
            CreateMap<BeneficiaryPayViewModel, Transaction>()
                .ForMember(dest => dest.SourceProductId, opt => opt.MapFrom(src => src.AccountId))
                .ForMember(dest => dest.DestinationProductId, opt => opt.MapFrom(src => src.BeneficiaryAccountId))
                .ForMember(dest => dest.Monto, opt => opt.MapFrom(src => src.Monto))
                .ForMember(dest => dest.Tipo, opt => opt.Ignore()) 
                .ForMember(dest => dest.Made, opt => opt.Ignore()) 
                .ForMember(dest => dest.Id, opt => opt.Ignore()); 
            #endregion
            #region AddCreditCard Mapping
            CreateMap<AddCreditCardViewModel, CreditCard>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Limit, opt => opt.MapFrom(src => src.Limit))
                .ForMember(dest => dest.Id, opt => opt.Ignore()); 
            #endregion
            #region UserResponse to AccountViewModel
            CreateMap<UserResponse, AccountViewModel>()
                .ForMember(dest => dest.UserSavingAccounts, opt => opt.Ignore())
                .ForMember(dest => dest.UserLoans, opt => opt.Ignore())
                .ForMember(dest => dest.UserCreditCards, opt => opt.Ignore())
                .ForMember(dest => dest.Cedula, opt => opt.MapFrom(src => src.Cedula));
            #endregion
            #region EditUser Mapping
            CreateMap<EditUserViewModel, EditUserRequest>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.ConfirmPassword, opt => opt.MapFrom(src => src.ConfirmPassword))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Cedula, opt => opt.MapFrom(src => src.Cedula))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ReverseMap()
                .ForMember(vm => vm.Monto, opt => opt.Ignore())
                .ForMember(vm => vm.IsAdmin, opt => opt.Ignore());
            #endregion
            #region Register to SavingAccount
            CreateMap<RegisterViewModel, AddSavingAccountViewModel>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
            #endregion
            #region Register
            CreateMap<RegisterRequest, RegisterViewModel>()
                .ForMember(vm => vm.Amount, opt => opt.Ignore())
                .ReverseMap();
            #endregion

            #region SavingAccount
            CreateMap<SavingAccount, SavingAccountViewModel>()
				.ReverseMap();
			#endregion

			#region CreditCard
			CreateMap<CreditCard, CreditCardViewModel>()
				.ForMember(vm => vm.Debt, opt => opt.Ignore())
				.ReverseMap()
				;
            

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
