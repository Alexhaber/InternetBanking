using InternetBanking.Core.Application.Dtos.User;
using InternetBanking.Core.Application.ViewModels.Account;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<LoginViewModel> AuthenticateAsync(LoginViewModel vm);
        Task SignOutAsync();
        Task<UserResponse> GetUserById(string id);
        Task<RegisterResponse> RegisterUserAsync(RegisterRequest request);
        Task<ICollection<string>> GetUserRolesByUserId(string id);
        Task<EditUserResponse> EditUserAsync(EditUserRequest request);

    }
}