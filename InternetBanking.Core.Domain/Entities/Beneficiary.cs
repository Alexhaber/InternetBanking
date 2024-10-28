namespace InternetBanking.Core.Domain.Entities
{
	public class Beneficiary
	{
		public int OwnerAccountId { get; set; }
        public SavingAccount OwnerAccount { get; set; }
        public int BeneficiaryAccountId { get; set; }
        public SavingAccount BeneficiaryAccount { get; set; }
    }
}
