using System.ComponentModel.DataAnnotations;

namespace DentAssist.Models.Entities
{
    public class Tratamiento
    {
        public Guid Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public int Costo { get; set; }

        public int CantidadSesiones { get; set; }

        [Required]
        public Guid PacienteId { get; set; }
        public Paciente? Paciente { get; set; }

        [Required]
        public Guid OdontologoId { get; set; }
        public Odontologo? Odontologo { get; set; }
    }

}
