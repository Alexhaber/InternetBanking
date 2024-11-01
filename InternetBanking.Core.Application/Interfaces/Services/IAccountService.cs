using InternetBanking.Core.Application.ViewModels.Account;

namespace InternetBanking.Core.Application.Interfaces.Services
{
	public interface IAccountService
	{
		Task<LoginViewModel> AuthenticateAsync(LoginViewModel vm);
		Task SignOutAsync();
	}
}
