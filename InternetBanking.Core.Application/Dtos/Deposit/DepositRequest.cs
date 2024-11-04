namespace InternetBanking.Core.Application.Dtos.Deposit
{
    public class DepositRequest
	{
		public string OriginProductId { get; set; }
		public string DestinyProductId { get; set; }
		public decimal Amount { get; set; }
	}
}
