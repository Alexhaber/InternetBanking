using InternetBanking.Infraestructure.Persistence.Contexts;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Core.Application.Dtos.Deposit;

namespace InternetBanking.Infraestructure.Persistence.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
	{

		private readonly AppDbContext _context;

		public TransactionRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}
	}
}
