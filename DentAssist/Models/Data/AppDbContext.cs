using DentAssist.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DentAssist.Models.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<Odontologo> Odontologos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Recepcionista> Recepcionistas { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Tratamiento> Tratamientos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Evita múltiples cascadas
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Paciente)
                .WithMany()
                .HasForeignKey(c => c.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Odontologo)
                .WithMany()
                .HasForeignKey(c => c.OdontologoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tratamiento>()
                .HasOne(c => c.Paciente)
                .WithMany()
                .HasForeignKey(c => c.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tratamiento>()
                .HasOne(c => c.Odontologo)
                .WithMany()
                .HasForeignKey(c => c.OdontologoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
