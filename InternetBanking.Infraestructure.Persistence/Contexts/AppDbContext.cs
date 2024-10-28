using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infraestructure.Persistence.DbConfiguration;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Infraestructure.Persistence.Contexts
{
	public class AppDbContext : DbContext
	{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<SavingAccount> SavingAccounts { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Loan> Loan { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Beneficiary> Beneficiaries { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ConfigDb();
		}
	}
}
