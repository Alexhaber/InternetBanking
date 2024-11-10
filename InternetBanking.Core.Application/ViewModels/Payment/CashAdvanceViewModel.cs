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
        [Range(0.0001, double.MaxValue, ErrorMessage = "Monto must be greater than 0")]
        public decimal Monto { get; set; }
        public List<CreditCardViewModel>? CreditCards { get; set; }

        public List<SavingAccountViewModel>? Accounts { get; set; }
    }
}
