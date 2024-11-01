namespace InternetBanking.Core.Application.ViewModels.CreditCard
{
	public class CreditCardViewModel
	{
		public string Id { get; set; }
		public string UserId { get; set; }
		public decimal Limit { get; private set; }
		public decimal Monto { get; set; }
		public decimal Debt => Limit - Monto;
    }
}
