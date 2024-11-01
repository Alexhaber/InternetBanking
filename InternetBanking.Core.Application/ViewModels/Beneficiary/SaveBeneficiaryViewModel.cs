using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Application.ViewModels.Beneficiary
{
	public class SaveBeneficiaryViewModel
    {
		[DataType(DataType.Text)]
		public string UserId { get; set; }

		[Required(ErrorMessage = "Numero de cuenta del beneficiario es requerida")]
		[DataType(DataType.Text)]
		public string BeneficiaryAccountId { get; set; }
		public bool HasError { get; set; }
		public string? Error { get; set; }
	}
}
