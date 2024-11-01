using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infraestructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Infraestructure.Persistence.Repositories
{
	public class LoanRepository : GenericRepository<Loan>, ILoanRepository
	{
		private readonly AppDbContext _context;

		public LoanRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<Loan>> GetLoansByClientIdAsync(string clientId)
		{
			return await _context.Loan.Where(l => l.UserId == clientId).ToListAsync();
		}
	}
}
