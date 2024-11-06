using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Application.ViewModels.Account
{
    public class EditUserViewModel
    {
        [Required(ErrorMessage = "El ID es obligatorio.")]
        public string Id { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El primer nombre es obligatorio.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "La cédula es obligatoria.")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "La confirmación de la contraseña es obligatoria.")]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

        public bool IsAdmin { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El monto debe ser positivo.")]
        public decimal Monto { get; set; }
    }
}