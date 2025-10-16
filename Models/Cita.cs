using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DASALUD.Models
{
    [Table("CITAS")]
    public class Cita
    {
        [Key]
        [Column("idCita")]
        public int IdCita { get; set; }

        [Required]
        [Column("idPaciente")]
        public int IdPaciente { get; set; }

        [Required]
        [Column("idEmpleado")]
        public int IdEmpleado { get; set; }

        [Required]
        [Column("idEstado")]
        public int IdEstado { get; set; }

        [Required]
        [Column("fechaCita")]
        public DateTime FechaCita { get; set; }

        [Required]
        [Column("costo", TypeName = "decimal(12,2)")]
        public decimal Costo { get; set; } = 0;

        [Column("activo")]
        public bool Activo { get; set; } = true;

        public Paciente Paciente { get; set; }
        public Empleado Empleado { get; set; }
        public EstadoCita Estado { get; set; }
    }
}
