using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DASALUD.Models
{
    [Table("ESTADOS_CITAS")]
    public class EstadoCita
    {
        [Key]
        [Column("idEstado")]
        public int IdEstado { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nombreEstado")]
        public string NombreEstado { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;

        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
