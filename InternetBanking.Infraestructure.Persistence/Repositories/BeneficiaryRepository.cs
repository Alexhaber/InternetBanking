using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infraestructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Infraestructure.Persistence.Repositories
{
	public class BeneficiaryRepository : GenericRepository<Beneficiary>, IBeneficiaryRepository
	{
		private readonly AppDbContext _context;

		public BeneficiaryRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<Beneficiary>> GetBeneficiariesByClientIdAsync(string clientId)
		{
			return await _context.Beneficiaries.Where(b => b.UserId == clientId).ToListAsync();
		}

		public async Task<Beneficiary?> GetBeneficiaryByIdAsync(string userId, string beneficiaryId)
		{
			return await _context.Beneficiaries.FirstOrDefaultAsync(b => b.UserId == userId && b.BeneficiaryAccountId == beneficiaryId);
		}
	}
}
