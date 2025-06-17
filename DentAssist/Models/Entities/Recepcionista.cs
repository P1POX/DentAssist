using Microsoft.AspNetCore.Identity;

namespace DentAssist.Models.Entities
{
    public class Recepcionista
    {
        public Guid Id { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string UserId { get; set; }
        public IdentityUser? User { get; set; }
    }
}
