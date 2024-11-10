using InternetBanking.Core.Application.ViewModels.Home;

namespace InternetBanking.Core.Application.Interfaces.Services
{
	public interface IHomeService
	{
		Task<HomeClientViewModel> GetProductsByClientIdAsync(string clientId);
		Task<DashBoardViewModel> GetDashBoardAsync();

    }
}
