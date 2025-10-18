using System.ComponentModel.DataAnnotations;

namespace DASALUD.ViewModels
{
    public class EditCitaViewModel
    {
        public int IdCita { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un paciente")]
        [Display(Name = "Paciente")]
        public int IdPaciente { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un médico")]
        [Display(Name = "Médico encargado")]
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "El costo es requerido")]
        [Range(0.01, 10000, ErrorMessage = "El costo debe ser mayor a 0")]
        [Display(Name = "Costo de Cita")]
        public decimal Costo { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        [Display(Name = "Fecha de la cita")]
        public DateTime FechaCita { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un estado")]
        [Display(Name = "Estado")]
        public int IdEstado { get; set; }

        // Listas para dropdowns
        public List<PacienteSelectItem>? Pacientes { get; set; }
        public List<EmpleadoSelectItem>? Empleados { get; set; }
        public List<EstadoCitaSelectItem>? Estados { get; set; }
    }

    public class EstadoCitaSelectItem
    {
        public int IdEstado { get; set; }
        public string NombreEstado { get; set; } = "";
    }
}
