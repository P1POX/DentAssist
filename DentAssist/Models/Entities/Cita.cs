using System.ComponentModel.DataAnnotations;

namespace DentAssist.Models.Entities
{
    public class Cita
    {
        public Guid Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public TimeSpan Duracion { get; set; }

        [Required]
        public Guid PacienteId { get; set; }
        public Paciente? Paciente { get; set; }

        [Required]
        public Guid OdontologoId { get; set; }
        public Odontologo? Odontologo { get; set; }

        [Required]
        public EstadoCita Estado { get; set; } = EstadoCita.Pendiente;

        public string? Observaciones { get; set; }
    }

    public enum EstadoCita
    {
        Pendiente,
        Confirmado,
        Realizado,
        Cancelado
    }

}
