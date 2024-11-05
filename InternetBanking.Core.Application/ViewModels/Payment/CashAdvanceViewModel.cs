using InternetBanking.Core.Application.ViewModels.CreditCard;
using InternetBanking.Core.Application.ViewModels.SavingAccount;
using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Application.ViewModels.Payment
{
    public class CashAdvanceViewModel
    {
        [Required(ErrorMessage = "Credit card is required")]
        [DataType(DataType.Text)]
        public string SenderProductId { get; set; }

        [Required(ErrorMessage = "Account is required")]
        [DataType(DataType.Text)]
        public string RecieverProductId { get; set; }

        [Required(ErrorMessage = "Monto is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Monto must be greater than 0")]
        [DataType(DataType.Currency)]
        public decimal Monto { get; set; }
        public List<CreditCardViewModel>? CreditCards { get; set; }

        public List<AccountViewModel>? Accounts { get; set; }
    }
}
