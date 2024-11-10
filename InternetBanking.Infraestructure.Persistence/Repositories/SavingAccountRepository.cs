using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infraestructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InternetBanking.Infraestructure.Persistence.Repositories
{
	public class SavingAccountRepository : GenericRepository<SavingAccount>, ISavingAccountRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SavingAccountRepository> _logger;

        public SavingAccountRepository(AppDbContext context, ILogger<SavingAccountRepository> logger): base(context) 
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<SavingAccount>> GetAccountsByClientIdAsync(string clientId)
        {
            // Asegurarse de que _context.SavingAccounts no es null.
            if (_context.SavingAccounts == null)
            {
                _logger.LogWarning("SavingAccounts DbSet is null. Ensure the database context is correctly configured.");
                return new List<SavingAccount>();
            }

            // Obtener las cuentas de ahorro del cliente.
            var cuentas = await _context.SavingAccounts
                                         .Where(a => a.UserId == clientId)
                                         .ToListAsync();

            // Registrar si no se encontraron cuentas.
            if (cuentas == null || cuentas.Count == 0)
            {
                _logger.LogInformation($"No saving accounts found for client ID {clientId}.");
                return new List<SavingAccount>(); // Devuelve una lista vacía en lugar de null
            }

            return cuentas;
        }

    }
}
