using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Application.ViewModels.Account
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Email is required")]
		[DataType(DataType.Text)]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public bool HasError { get; set; }
		public string? Error { get; set; }
		public List<string>? Roles { get; set; }
	}
}
