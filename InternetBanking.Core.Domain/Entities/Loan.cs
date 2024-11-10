using InternetBanking.Core.Domain.Common;

namespace InternetBanking.Core.Domain.Entities
{
	public class Loan : AuditableBaseEntity
	{
        
        public decimal Monto { get;  set; }
        public decimal Paid { get; set; }

        public Loan(decimal monto)
        {
            Monto = monto;
        }
    }
}
