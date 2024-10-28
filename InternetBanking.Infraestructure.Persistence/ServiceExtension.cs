using InternetBanking.Infraestructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
		}
	}
}
