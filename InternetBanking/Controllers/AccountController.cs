﻿using AutoMapper;
using InternetBanking.Core.Application.Dtos.User;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Account;
using InternetBanking.Core.Application.ViewModels.SavingAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Controllers
{
	public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ISavingAccountService _savingAccountService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, ISavingAccountService savingAccountService, IMapper mapper)
        {
            _accountService = accountService;
            _savingAccountService = savingAccountService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Register()
        {
            if (User.IsInRole("Client"))
            {
                return RedirectToAction("AdminIndex", "Home");
            }
            return View(new RegisterViewModel());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid && model.Amount >= 0)
            {
                return View(model);
            }

            var newClient = _mapper.Map<RegisterRequest>(model);

            var response = await _accountService.RegisterUserAsync(newClient);

            if (response.HasError)
            {
                ModelState.AddModelError(string.Empty, response.Error);
                return View(model);
            }

            if(!model.Admin)
            {
                AddSavingAccountViewModel savingAccount = _mapper.Map<AddSavingAccountViewModel>(model);
                savingAccount.Amount = model.Amount;
                savingAccount.UserId = response.IdCreatedUser;
                

                await _savingAccountService.AddSavingAccountAsync(savingAccount);
            }

            return RedirectToAction("AdminIndex", "Home");
        }


        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("AdminIndex", "Home"); //vista de admin
                }

                return RedirectToAction("Client", "Home"); //vista de cliente
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Dashboard", "Home"); //vista de admin
                }

                return RedirectToAction("Client", "Home"); //vista de cliente
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var response = await _accountService.AuthenticateAsync(vm);

            if (response.HasError)
            {
                return View(response);
            }

            if (response.Roles.Contains("Admin"))
            {
                return RedirectToAction("Dashboard", "Home"); //vista de admin
            }

            return RedirectToAction("Client", "Home"); //vista de cliente
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditAccount(string id)
        {
            var user = await _accountService.GetUserById(id);
            if (user == null)
            {
                // Redirige o muestra un mensaje de error si el usuario no se encuentra
                return RedirectToAction("Error", "Home", new { message = "User not found." });
            }
            ICollection<string> userRoles = await _accountService.GetUserRolesByUserId(user.Id);
            if (userRoles.Count == 0)
            {
                return RedirectToAction("Index", "Home"); //ruta provisional realmente se redirige a vista de Administrador(Index)
            }

            EditUserViewModel vm = new()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAdmin = false,
                Cedula = user.Cedula,
                Monto = 0,
                Email = user.Email,

            };
            if (userRoles.Contains("Admin"))
            {
                vm.IsAdmin = true;
            }
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditAccount(EditUserViewModel user)
        {
            if (!ModelState.IsValid || user == null)
            {
                return View(user);
            }

            try
            {

                EditUserRequest request = _mapper.Map<EditUserRequest>(user);



                var editResponse = await _accountService.EditUserAsync(request);

                
                if (editResponse.HasError)
                {
                    ModelState.AddModelError(string.Empty, editResponse.Error);
                    return View(user); 
                }

                
                if (!user.IsAdmin)
                {
                    var monto = user.Monto;
                    await _savingAccountService.AdminIncreaseMainSavingAccountAmmount(monto, user.Id);
                }

                
                return RedirectToAction("AdminIndex", "Home");
            }
            catch (InvalidOperationException ex)
            {
                // Maneja errores específicos de lógica del negocio
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                
                ModelState.AddModelError(string.Empty, "Se produjo un error al intentar editar la cuenta.");
                
            }

            return View(user); 
        }



        public async Task<IActionResult> LogOut()
        {
            await _accountService.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        
    }
}