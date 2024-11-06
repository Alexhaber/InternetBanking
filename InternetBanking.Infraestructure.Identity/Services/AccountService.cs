using AutoMapper.Configuration.Annotations;
using Azure;
using InternetBanking.Core.Application.Dtos.User;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Account;
using InternetBanking.Infraestructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace InternetBanking.Infraestructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<LoginViewModel> AuthenticateAsync(LoginViewModel vm)
        {
            LoginViewModel respond = new();

            var user = await _userManager.FindByEmailAsync(vm.Email);

            if (user == null)
            {
                respond.HasError = true;
                respond.Error = $"No Accounts registered with {vm.Email}";
                return respond;
            }

            if (!user.EmailConfirmed)
            {
                respond.HasError = true;
                respond.Error = $"Account no confirmed for {vm.Email}";
                return respond;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, vm.Password, false, false);

            if (!result.Succeeded)
            {
                respond.HasError = true;
                respond.Error = $"Invalid credential for {vm.Email}";
                return respond;
            }

            var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            respond.Roles = roles.ToList();

            return respond;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<UserResponse> GetUserById(string id)
        {
            // Asegúrate de que _userManager esté correctamente inicializado.
            if (_userManager == null)
            {
                throw new InvalidOperationException("UserManager is not initialized.");
            }

            // Busca el usuario por su ID.
            var user = await _userManager.FindByIdAsync(id);

            // Verifica si el usuario es nulo y maneja el caso.
            if (user == null)
            {
                return null; // O devuelve una respuesta que indique que el usuario no fue encontrado.
            }

            return new UserResponse
            {
                Id = user.Id,
                UserName = user.UserName ?? "Unknown", // Usa valores predeterminados si es necesario
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                // Otras propiedades...
            };
        }


        public async Task<ICollection<string>> GetUserRolesByUserId(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }

        public async Task<EditUserResponse> EditUserAsync(EditUserRequest request)
        {
            EditUserResponse response = new()
            {
                HasError = false,
            };

            var userWithSameUserName = await _userManager.FindByNameAsync(request.FirstName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"username '{request.FirstName}' is already taken.";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                if (userWithSameEmail.Id != request.Id)
                {
                    response.HasError = true;
                    response.Error = $"Email '{request.Email}' is already registered.";
                    return response;
                }

            }

            if (request.Password != request.ConfirmPassword)
            {
                response.HasError = true;
                response.Error = $"Password and Confirm Passord are not the same";
                return response;
            }

            try
            {
                var existingUser = await _userManager.FindByIdAsync(request.Id);
                existingUser.FirstName = request.FirstName;
                existingUser.Email = request.Email;
                existingUser.LastName = request.LastName;
                existingUser.Cedula = request.Cedula;
                existingUser.UserName = request.UserName;
                await _userManager.UpdateAsync(existingUser);
                return response;
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Error = ex.Message;
                return response;
            }


        }
        public async Task<RegisterResponse> RegisterUserAsync(RegisterRequest request)
        {

            RegisterResponse response = new()
            {
                HasError = false
            };

            var userWithSameUserName = await _userManager.FindByNameAsync(request.FirstName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"username '{request.FirstName}' is already taken.";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"Email '{request.Email}' is already registered.";
                return response;
            }

            var user = new AppUser
            {
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailConfirmed = true,


            };
            var result = await _userManager.CreateAsync(user, request.Password);
            var createdUser = await _userManager.FindByEmailAsync(user.Email);
            if (result.Succeeded)
            {
                if (!request.Admin)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Client.ToString());
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                }
                response.IdCreatedUser = createdUser.Id;
                return response;
            }
            else
            {
                response.HasError = true;
                response.Error = $"An error occurred trying to register the user.";
                return response;
            }
        }

    }
}