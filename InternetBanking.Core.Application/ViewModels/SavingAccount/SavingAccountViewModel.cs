namespace InternetBanking.Core.Application.ViewModels.SavingAccount
{
	public class SavingAccountViewModel
	{
        public string Id { get; set; }
        public string UserId { get; set; }
		public decimal Monto { get; set; }
		public bool IsPrincipal { get; set; }
	}
}
