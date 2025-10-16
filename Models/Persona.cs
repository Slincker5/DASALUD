using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DASALUD.Models
{
    [Table("PERSONAS")]
    public class Persona
    {
        [Key]
        [Column("idPersona")]
        public int IdPersona { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nombres")]
        public string Nombres { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("apellidos")]
        public string Apellidos { get; set; } = string.Empty;

        [MaxLength(255)]
        [Column("correo")]
        public string? Correo { get; set; }

        [MaxLength(20)]
        [Column("DUI")]
        public string? DUI { get; set; }

        [MaxLength(30)]
        [Column("telefono")]
        public string? Telefono { get; set; }

        [MaxLength(255)]
        [Column("direccion")]
        public string? Direccion { get; set; }
    }
}
