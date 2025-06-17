using Microsoft.AspNetCore.Identity;

namespace DentAssist.Models.Entities
{
    public class Paciente
    {
        public Guid Id { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string RUT { get; set; }
        public required string Direccion { get; set; }
        public required string UserId { get; set; }
        public IdentityUser? User { get; set; }
    }

}
