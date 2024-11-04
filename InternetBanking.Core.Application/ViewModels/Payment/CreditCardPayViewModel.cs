using InternetBanking.Core.Application.ViewModels.CreditCard;
using InternetBanking.Core.Application.ViewModels.SavingAccount;
using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Application.ViewModels.Payment
{
	public class CreditCardPayViewModel
	{
		[Required(ErrorMessage = "Credit Card is required")]
		[DataType(DataType.Text)]
		public string CreditCardId { get; set; }

		[Required(ErrorMessage = "Account is required")]
		[DataType(DataType.Text)]
		public string AccountId { get; set; }

		[Required(ErrorMessage = "Monto es requerido")]
		[Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
		[DataType(DataType.Currency)]
		public decimal Monto { get; set; }
		public bool HasError { get; set; }
		public string? Error { get; set; }
		public bool IsSucceeded { get; set; }
        public List<CreditCardViewModel>? CreditCards { get; set; }
        public List<AccountViewModel>? Accounts { get; set; }
    }
}
