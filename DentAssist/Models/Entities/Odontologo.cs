using Microsoft.AspNetCore.Identity;

namespace DentAssist.Models.Entities
{
    public class Odontologo
    {
        public Guid Id { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string Matricula { get; set; }
        public Guid EspecialidadId { get; set; }
        public Especialidad? Especialidad { get; set; }
        public required string UserId { get; set; }
        public IdentityUser? User { get; set; }
    }
}
