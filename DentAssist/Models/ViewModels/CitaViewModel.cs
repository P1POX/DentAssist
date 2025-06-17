using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DentAssist.Models.ViewModels
{
    public class CitaViewModel
    {
        [Required]
        [Display(Name = "Fecha y hora")]
        public DateTime Fecha { get; set; }

        [Required]
        [Display(Name = "Duración (minutos)")]
        public int DuracionMinutos { get; set; }

        [Required]
        [Display(Name = "Paciente")]
        public Guid PacienteId { get; set; }

        [Required]
        [Display(Name = "Odontólogo")]
        public Guid OdontologoId { get; set; }

        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        public IEnumerable<SelectListItem>? Pacientes { get; set; }
        public IEnumerable<SelectListItem>? Odontologos { get; set; }
    }
}
