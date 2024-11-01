using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infraestructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Infraestructure.Persistence.Repositories
{
	public class CreditCardRepository : GenericRepository<CreditCard>, ICreditCardRepository
	{
		private readonly AppDbContext _context;

		public CreditCardRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<CreditCard>> GetCreditCardsByClientIdAsync(string clientId)
		{
			return await _context.CreditCards.Where(c => c.UserId == clientId).ToListAsync();
		}
	}
}
