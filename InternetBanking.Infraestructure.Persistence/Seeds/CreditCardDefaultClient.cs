using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infraestructure.Persistence.Contexts;

namespace InternetBanking.Infraestructure.Persistence.Seeds
{
	public class CreditCardDefaultClient
	{
		public static async Task SeedAsync(AppDbContext context, string userId)
		{
			var creditCard = new CreditCard(5000m);
			creditCard.Id = "098765432";
			creditCard.UserId = userId;

			var card = await context.CreditCards.FindAsync(creditCard.Id);

			if(card == null)
			{
				await context.CreditCards.AddAsync(creditCard);
				await context.SaveChangesAsync();
			}
			
		}
	}
}
