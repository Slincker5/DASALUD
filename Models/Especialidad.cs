using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DASALUD.Models
{
    [Table("ESPECIALIDADES")]
    public class Especialidad
    {
        [Key]
        [Column("idEspecialidad")]
        public int IdEspecialidad { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("nombreEspecialidad")]
        public string NombreEspecialidad { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;

        public ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
    }
}
