using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DASALUD.Models
{
    [Table("EMPLEADOS")]
    public class Empleado
    {
        [Key]
        [Column("idEmpleado")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdEmpleado { get; set; }

        [Column("idRol")]
        [Required]
        public int IdRol { get; set; }

        [Column("idEspecialidad")]
        [Required]
        public int IdEspecialidad { get; set; }

        [Column("usuario")]
        [Required]
        [StringLength(255)]
        public string Usuario { get; set; } = string.Empty;

        [Column("contrasena")]
        [Required]
        [StringLength(255)]
        public string Contrasena { get; set; } = string.Empty;

        [Column("activo")]
        public bool Activo { get; set; } = true;

        [ForeignKey("IdEmpleado")]
        public virtual Persona Persona { get; set; } = null!;

        [ForeignKey("IdRol")]
        public virtual Rol Rol { get; set; } = null!;

        [ForeignKey("IdEspecialidad")]
        public virtual Especialidad Especialidad { get; set; } = null!;

        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
