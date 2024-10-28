using InternetBanking.Core.Domain.Common;

namespace InternetBanking.Core.Domain.Entities
{
	public class Loan : AuditableBaseEntity
	{
        public decimal Monto { get; }
        public decimal Paid { get; set; }
        public decimal Debt => Monto - Paid;

        public Loan(decimal monto)
        {
            Monto = monto;
        }
    }
}
