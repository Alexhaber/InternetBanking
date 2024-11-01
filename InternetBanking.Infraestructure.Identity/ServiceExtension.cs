using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Infraestructure.Identity.Contexts;
using InternetBanking.Infraestructure.Identity.Entities;
using InternetBanking.Infraestructure.Identity.Seeds;
using InternetBanking.Infraestructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InternetBanking.Infraestructure.Identity
{
	public static class ServiceExtension
	{
		public static void AddIdentityInfraestructureLayer(this IServiceCollection services, IConfiguration config)
		{
			#region Contexts
			if (config.GetValue<bool>("UseInMemoryDatabase"))
			{
				services.AddDbContext<IdentityContext>(options => options.UseInMemoryDatabase("IdentityDbInMemory"));
			}
			else
			{
				services.AddDbContext<IdentityContext>(options =>
				{
					options.EnableSensitiveDataLogging();
					options.UseSqlServer(config.GetConnectionString("IdentityDbConnection"),
						m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));
				});
			}
			#endregion

			#region Identity
			services.AddIdentity<AppUser, IdentityRole>()
					.AddEntityFrameworkStores<IdentityContext>()
					.AddDefaultTokenProviders();

			services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/Account/Login";
				options.AccessDeniedPath = "/Account/AccessDenied";
			});

			services.AddAuthentication();
			#endregion

			#region Services
			services.AddTransient<IAccountService, AccountService>();
			#endregion
		}

		public static async Task SeedIdentityDbAsync(this IServiceProvider serviceProvider)
		{
			using (var scope = serviceProvider.CreateScope())
			{
				var services = scope.ServiceProvider;

				try
				{
					var userManager = services.GetRequiredService<UserManager<AppUser>>();
					var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

					await DefaultRoles.SeedAsync(roleManager);
					await DefaultAdmin.SeedAsync(userManager);
					await DefaultClient.SeedAsync(userManager);
				}
				catch (Exception e)
				{

				}
			}
		}
	}
}
