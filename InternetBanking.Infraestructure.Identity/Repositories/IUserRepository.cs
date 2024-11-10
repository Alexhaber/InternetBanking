using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Infraestructure.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Infraestructure.Identity.Repositories
{
    public interface IUserRepository: IGenericRepository<AppUser>
    {
    }
}
