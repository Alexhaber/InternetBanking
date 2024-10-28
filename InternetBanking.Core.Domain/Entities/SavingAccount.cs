using InternetBanking.Core.Domain.Common;

namespace InternetBanking.Core.Domain.Entities
{
	public class SavingAccount : AuditableBaseEntity
	{
        public decimal Monto { get; set; }
        public bool IsPrincipal { get; set; }
        public List<Beneficiary> Beneficiaries { get; set; }
    }
}
