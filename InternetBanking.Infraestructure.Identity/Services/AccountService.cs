using AutoMapper.Configuration.Annotations;
using Azure;
using InternetBanking.Core.Application.Dtos.User;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Account;
using InternetBanking.Infraestructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace InternetBanking.Infraestructure.Identity.Services
{
    public class AccountService : IAccountService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;

		public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public async Task<LoginViewModel> AuthenticateAsync(LoginViewModel vm)
		{
			LoginViewModel respond = new();

			var user = await _userManager.FindByEmailAsync(vm.Email);

			if (user == null)
			{
				respond.HasError = true;
				respond.Error = $"No Accounts registered with {vm.Email}";
				return respond;
			}

			if (!user.EmailConfirmed)
			{
				respond.HasError = true;
				respond.Error = $"Account no confirmed for {vm.Email}";
				return respond;
			}

			var result = await _signInManager.PasswordSignInAsync(user.UserName, vm.Password, false, false);

			if (!result.Succeeded)
			{
				respond.HasError = true;
				respond.Error = $"Invalid credential for {vm.Email}";
				return respond;
			}

			var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

			respond.Roles = roles.ToList();

			return respond;
		}

		public async Task SignOutAsync()
		{
			await _signInManager.SignOutAsync();
		}
		public async Task<UserResponse> GetUserById(string id)
		{
			var user = await _userManager.FindByIdAsync(id);

			return new UserResponse
			{
				Id = id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Cedula = user.Cedula,
				Email = user.Email
			};
		}
	}
}
