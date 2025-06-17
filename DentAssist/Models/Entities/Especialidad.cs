namespace DentAssist.Models.Entities
{
    public class Especialidad
    {
        public Guid Id { get; set; }
        public required string Nombre { get; set; }
        public ICollection<Odontologo>? Odontologos { get; set; }
    }

}
