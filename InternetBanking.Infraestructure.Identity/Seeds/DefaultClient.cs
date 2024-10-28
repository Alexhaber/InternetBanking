using InternetBanking.Core.Application.Enums;
using InternetBanking.Infraestructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace InternetBanking.Infraestructure.Identity.Seeds
{
	public class DefaultClient
	{
		public static async Task SeedAsync(UserManager<AppUser> userManager)
		{
			var defaultUser = new AppUser
			{
				FirstName = "DefaultClient",
				LastName = "DefaultClient",
				UserName = "defaulClient",
				Cedula = "000-0000000-0",
				Email = "cliente@gmail.com",
				EmailConfirmed = true
			};

			var user = await userManager.FindByEmailAsync(defaultUser.Email);

			if (user == null)
			{
				await userManager.CreateAsync(defaultUser, "123Pa$$word!");
				await userManager.AddToRoleAsync(defaultUser, Roles.Client.ToString());
			}
		}
	}
}
