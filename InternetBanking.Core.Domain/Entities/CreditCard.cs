using InternetBanking.Core.Domain.Common;

namespace InternetBanking.Core.Domain.Entities
{
	public class CreditCard : AuditableBaseEntity
	{
        public decimal Limit { get; private set; }
		public decimal Monto { get; set; }

        public CreditCard(decimal limit)
        {
            Limit = limit;
            Monto = Limit;
        }
    }
}
