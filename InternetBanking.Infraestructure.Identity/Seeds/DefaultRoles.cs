using InternetBanking.Core.Application.Enums;
using Microsoft.AspNetCore.Identity;

namespace InternetBanking.Infraestructure.Identity.Seeds
{
	public class DefaultRoles
	{
		public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
		{
			await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
			await roleManager.CreateAsync(new IdentityRole(Roles.Client.ToString()));
		}
	}
}
