using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infraestructure.Persistence.Contexts;

namespace InternetBanking.Infraestructure.Persistence.Repositories
{
	public class BeneficiaryRepository : GenericRepository<Beneficiary>, IBeneficiaryRepository
	{
		private readonly AppDbContext _context;

		public BeneficiaryRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}
	}
}
