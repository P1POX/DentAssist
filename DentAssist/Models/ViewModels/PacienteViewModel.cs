using System.ComponentModel.DataAnnotations;

namespace DentAssist.Models.ViewModels
{
    public class PacienteViewModel
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        [Display(Name = "RUT")]
        public string RUT { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;
    }
}
