namespace InternetBanking.Core.Application.Dtos.Deposit
{
	public class DepositResponse
	{
		public bool HasError { get; set; }
		public string? Error { get; set; }
	}
}
