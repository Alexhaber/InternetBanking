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
        public int TodayPayments { get; set; }
        public int AllPayments { get; set; }
        public int ActiveClients {  get; set; }
        public int InactiveClientsCount { get; set; }
        public int Products {  get; set; }
    }
}
