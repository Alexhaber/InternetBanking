using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infraestructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Infraestructure.Persistence.Repositories
{
	public class SavingAccountRepository : GenericRepository<SavingAccount>, ISavingAccountRepository
    {
        private readonly AppDbContext _context;

		public SavingAccountRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<SavingAccount>> GetAccountsByClientIdAsync(string clientId)
		{
			return await _context.SavingAccounts.Where(a => a.UserId == clientId).ToListAsync();
		}
	}
}
