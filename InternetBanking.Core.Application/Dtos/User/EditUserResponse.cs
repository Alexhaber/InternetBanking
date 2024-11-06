using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Dtos.User
{
    public class EditUserResponse
    {
        public string? Error { get; set; }
        public bool HasError { get; set; }
        public string? IdEditedUser { get; set; }
    }
}