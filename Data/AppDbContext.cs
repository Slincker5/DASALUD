using DASALUD.Models;
using Microsoft.EntityFrameworkCore;

namespace DASALUD.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Persona> Personas { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<EstadoCita> EstadosCitas { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Cita> Citas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Paciente>()
                .HasOne(p => p.Persona)
                .WithOne()
                .HasForeignKey<Paciente>(p => p.IdPaciente)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<Empleado>()
                .HasOne(e => e.Persona)
                .WithOne()
                .HasForeignKey<Empleado>(e => e.IdEmpleado)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<Empleado>()
                .HasIndex(e => e.Usuario)
                .IsUnique();

            modelBuilder.Entity<Empleado>()
                .HasOne(e => e.Rol)
                .WithMany(r => r.Empleados)
                .HasForeignKey(e => e.IdRol)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Empleado>()
                .HasOne(e => e.Especialidad)
                .WithMany(es => es.Empleados)
                .HasForeignKey(e => e.IdEspecialidad)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Paciente)
                .WithMany(p => p.Citas)
                .HasForeignKey(c => c.IdPaciente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Empleado)
                .WithMany(e => e.Citas)
                .HasForeignKey(c => c.IdEmpleado)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Estado)
                .WithMany(ec => ec.Citas)
                .HasForeignKey(c => c.IdEstado)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rol>()
                .HasIndex(r => r.NombreRol)
                .IsUnique();

            modelBuilder.Entity<Especialidad>()
                .HasIndex(e => e.NombreEspecialidad)
                .IsUnique();

            modelBuilder.Entity<EstadoCita>()
                .HasIndex(ec => ec.NombreEstado)
                .IsUnique();
        }
    }
}
