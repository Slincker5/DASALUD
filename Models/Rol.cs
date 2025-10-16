using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DASALUD.Models
{
    [Table("ROLES")]
    public class Rol
    {
        [Key]
        [Column("idRol")]
        public int IdRol { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nombreRol")]
        public string NombreRol { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;

        public ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
    }
}
