using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infraestructure.Persistence.Contexts;

namespace InternetBanking.Infraestructure.Persistence.Seeds
{
	public class SavingAccountDefaultClient
	{
		public static async Task SeedAsync(AppDbContext context)
		{
			var savingAccount = new SavingAccount
			{
				Id = "123456789",
				UserId = "01ab7241-ba4d-46c8-b62e-caad7abb5b9c",
				Monto = 10000m,
				IsPrincipal = true
			};

			var account = await context.SavingAccounts.FindAsync(savingAccount.Id);

			if(account == null)
			{
				await context.SavingAccounts.AddAsync(savingAccount);
				await context.SaveChangesAsync();
			}
		}
	}
}
