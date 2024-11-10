using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Infraestructure.Identity.Contexts;
using InternetBanking.Infraestructure.Identity.Entities;
using InternetBanking.Infraestructure.Persistence.Contexts;
using InternetBanking.Infraestructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Infraestructure.Identity.Repositories
{
    public class UserRepository : IdentityGenericRepository<AppUser>, IUserRepository
    {
        private readonly IdentityContext _context;

        public UserRepository(IdentityContext context) : base(context)
        {
            _context = context;
        }

        
    }
}
