using InternetBanking.Core.Application.ViewModels.Beneficiary;
using InternetBanking.Core.Application.ViewModels.SavingAccount;
using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Application.ViewModels.Payment
{
	public class BeneficiaryPayViewModel
    {

        [Required(ErrorMessage = "Loan is required")]
        [DataType(DataType.Text)]
        public string BeneficiaryAccountId { get; set; }

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
        public List<BeneficiaryViewModel>? Beneficiaries { get; set; }
        public List<SavingAccountViewModel>? Accounts { get; set; }
		public string? BeneficiaryName { get; set; }
	}
}
