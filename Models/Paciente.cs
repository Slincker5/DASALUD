using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DASALUD.Models
{
    [Table("PACIENTES")]
    public class Paciente
    {
        [Key]
        [Column("idPaciente")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public int IdPaciente { get; set; }

        [Column("tipoSangre")]
        [StringLength(5)]
        public string? TipoSangre { get; set; }

        [Column("peso")]
        public decimal? Peso { get; set; }

        [Column("altura")]
        public decimal? Altura { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;

        [ForeignKey("IdPaciente")]
        public virtual Persona Persona { get; set; } = null!;

        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
