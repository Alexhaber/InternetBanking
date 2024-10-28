using InternetBanking.Core.Application.Enums;
using InternetBanking.Infraestructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace InternetBanking.Infraestructure.Identity.Seeds
{
	public class DefaultAdmin
	{
		public static async Task SeedAsync(UserManager<AppUser> userManager)
		{
			var defaultUser = new AppUser
			{
				FirstName = "DefaultAdmin",
				LastName = "DefaultAdmin",
				UserName = "defaulAdmin",
				Cedula = "000-0000000-1",
				Email = "admin@gmail.com",
				EmailConfirmed = true
			};

			var user = await userManager.FindByEmailAsync(defaultUser.Email);

			if (user == null)
			{
				await userManager.CreateAsync(defaultUser, "123Pa$$word!");
				await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
			}
		}
	}
}
