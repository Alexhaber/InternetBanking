namespace InternetBanking.Core.Application.ViewModels.Loan
{
	public class LoanViewModel
	{
		public string Id { get; set; }
		public string UserId { get; set; }
		public decimal Monto { get; private set; }
		public decimal Paid { get; set; }
		public decimal Debt => Monto - Paid;
	}
}
