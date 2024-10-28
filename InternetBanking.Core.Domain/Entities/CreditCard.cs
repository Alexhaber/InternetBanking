using InternetBanking.Core.Domain.Common;

namespace InternetBanking.Core.Domain.Entities
{
	public class CreditCard : AuditableBaseEntity
	{
        public decimal Limit { get; }
		public decimal Monto { get; set; }
		public decimal Debt => Limit - Monto;

        public CreditCard(decimal limit)
        {
            Limit = limit;
            Monto = Limit;
        }
    }
}
