using InternetBanking.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Infraestructure.Persistence.DbConfiguration
{
	public static class DbConfig
	{
		public static void ConfigDb(this ModelBuilder modelBuilder)
		{
			#region SavingsAccount
			modelBuilder.Entity<SavingAccount>().ToTable("SavingAccounts");
			modelBuilder.Entity<SavingAccount>().HasKey(a => a.Id);
			modelBuilder.Entity<SavingAccount>().Property(a => a.Id).HasMaxLength(9).IsRequired();
			modelBuilder.Entity<SavingAccount>().Property(a => a.UserId).IsRequired();
			modelBuilder.Entity<SavingAccount>().Property(a => a.Monto).HasColumnType("decimal(12,4)").IsRequired();
			modelBuilder.Entity<SavingAccount>().Property(a => a.IsPrincipal).IsRequired();
			#endregion

			#region CreditCard
			modelBuilder.Entity<CreditCard>().ToTable("CreditCards");
			modelBuilder.Entity<CreditCard>().HasKey(c => c.Id);
			modelBuilder.Entity<CreditCard>().Property(c => c.Id).HasMaxLength(9).IsRequired();
			modelBuilder.Entity<CreditCard>().Property(c => c.UserId).IsRequired();
			modelBuilder.Entity<CreditCard>().Property(c => c.Limit).HasColumnType("decimal(12,4)").IsRequired();
			modelBuilder.Entity<CreditCard>().Property(c => c.Monto).HasColumnType("decimal(12,4)").IsRequired();
			#endregion

			#region Loan
			modelBuilder.Entity<Loan>().ToTable("Loans");
			modelBuilder.Entity<Loan>().HasKey(l => l.Id);
			modelBuilder.Entity<Loan>().Property(l => l.Id).HasMaxLength(9).IsRequired();
			modelBuilder.Entity<Loan>().Property(l => l.UserId).IsRequired();
			modelBuilder.Entity<Loan>().Property(l => l.Monto).HasColumnType("decimal(12,4)").IsRequired();
			modelBuilder.Entity<Loan>().Property(l => l.Paid).HasColumnType("decimal(12,4)").IsRequired();
			#endregion

			#region Beneficiary
			modelBuilder.Entity<Beneficiary>().ToTable("Beneficiaries");
			modelBuilder.Entity<Beneficiary>().HasKey(b => new {b.UserId, b.BeneficiaryAccountId});
			modelBuilder.Entity<Beneficiary>().Property(b => b.UserId).IsRequired();
			modelBuilder.Entity<Beneficiary>().Property(b => b.BeneficiaryAccountId).IsRequired();
			#endregion

			#region Transaction
			modelBuilder.Entity<Transaction>().ToTable("Transactions");
			modelBuilder.Entity<Transaction>().HasKey(t => t.Id);
			modelBuilder.Entity<Transaction>().Property(t => t.SourceProductId).IsRequired();
			modelBuilder.Entity<Transaction>().Property(t => t.DestinationProductId).IsRequired();
			modelBuilder.Entity<Transaction>().Property(t => t.Monto).HasColumnType("decimal(12,4)").IsRequired();
			modelBuilder.Entity<Transaction>().Property(t => t.Made).IsRequired();
			#endregion
		}
	}
}
