using Microsoft.AspNetCore.Identity;

namespace InternetBanking.Infraestructure.Identity.Entities
{
	public class AppUser : IdentityUser
	{
        public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Cedula { get; set; }
	}
}
