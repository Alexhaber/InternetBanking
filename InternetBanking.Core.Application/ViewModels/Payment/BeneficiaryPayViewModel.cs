﻿using InternetBanking.Core.Application.ViewModels.Beneficiary;
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

        [Required(ErrorMessage = "Monto is required")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "Monto must be greater than 0")]
        public decimal Monto { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
        public bool IsSucceeded { get; set; }
        public List<BeneficiaryViewModel>? Beneficiaries { get; set; }
        public List<SavingAccountViewModel>? Accounts { get; set; }
		public string? BeneficiaryName { get; set; }
	}
}
