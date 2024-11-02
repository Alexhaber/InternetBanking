using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Dtos.Deposit
{
	public class DepositRequest
	{
		public string originProduct { get; set; }
		public SavingAccount account { get; set; }
		public decimal amount { get; set; }
	}
}
