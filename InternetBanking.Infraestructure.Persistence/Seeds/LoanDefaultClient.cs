using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infraestructure.Persistence.Contexts;

namespace InternetBanking.Infraestructure.Persistence.Seeds
{
	public class LoanDefaultClient
	{
		public static async Task SeedAsync(AppDbContext context, string userId)
		{
			var loan = new Loan(5000m);
			loan.Id = "456789012";
			loan.UserId = userId;

			var loanExits = await context.Loan.FindAsync(loan.Id);

			if(loanExits == null)
			{
				await context.Loan.AddAsync(loan);
				await context.SaveChangesAsync();
			}
		}
	}
}
