using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Infraestructure.Persistence.Contexts;
using InternetBanking.Infraestructure.Persistence.Repositories;
using InternetBanking.Infraestructure.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InternetBanking.Infraestructure.Persistence
{
	public static class ServiceExtension
	{
		public static void AddPersistenceInfraestructureLayer(this IServiceCollection services, IConfiguration config)
		{
			#region Database Connection
			if (config.GetValue<bool>("InMemoryDatabase"))
			{
				services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("DbInMemory"));
			}
			else
			{
				services.AddDbContext<AppDbContext>(options => options.UseSqlServer(config.GetConnectionString("DbConnection"),
					m => m.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
			}
			#endregion

			#region Repositories
			services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			#endregion
		}

		public static async Task SeedSavingAccountDefaulClient(this IServiceProvider serviceProvider)
		{
			using (var scope = serviceProvider.CreateScope())
			{
				var services = scope.ServiceProvider;

				try
				{
					var context = services.GetRequiredService<AppDbContext>();

					await SavingAccountDefaultClient.SeedAsync(context);
				}
				catch (Exception e)
				{

				}
			}
		}
	}
}
