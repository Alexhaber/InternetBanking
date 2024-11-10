namespace InternetBanking.Core.Application.Dtos.User
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cedula { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
    }
}