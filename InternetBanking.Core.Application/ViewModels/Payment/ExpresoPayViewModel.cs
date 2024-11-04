﻿using InternetBanking.Core.Application.ViewModels.SavingAccount;
using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Application.ViewModels.Payment
{
    public class ExpresoPayViewModel
    {
        [Required(ErrorMessage = "Numero de cuenta donde quiere depositar es requerida")]
		[RegularExpression(@"^\d{9}$", ErrorMessage = "Numero de cuenta debe tener exactamente 9 dígitos numéricos")]
		[DataType(DataType.Text)]
        public string DestinyAccountId { get; set; }

        [Required(ErrorMessage = "Numero de cuenta origen es requerida")]
        [DataType(DataType.Text)]
        public string OriginAccountId { get; set; }

        [Required(ErrorMessage = "Monto es requerido")]
		[Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
		[DataType(DataType.Currency)]
        public decimal Monto { get; set; }
        public List<AccountViewModel>? Accounts { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
        public bool IsSucceeded { get; set; }
        public string? DestinyAccountOwnerName { get; set; }
    }
}
