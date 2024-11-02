using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Dtos.Deposit
{
	public class DepositResponse
	{
		//CreditCard card {  get; set; }
		//SavingAccount account { get; set; }
		//int amount { get; set; }
		public bool HasError { get; set; }
		public string? Error { get; set; }
	}
}
