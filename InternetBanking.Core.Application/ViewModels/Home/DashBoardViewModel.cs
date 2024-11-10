using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Home
{
    public class DashBoardViewModel
    {
        public int TransactionsCount { get; set; }
        public int TransactionsToday { get; set; }
        public int TotalPaymentsCount { get; set; }
        public int PaymentsTodayCount { get; set; }
        public int ActiveClientsCount { get; set; }
        public int InactiveClientsCount { get; set; }
        public int TotalProductsAssigned { get; set; }
    }

}
