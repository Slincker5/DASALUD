using System.ComponentModel.DataAnnotations;

namespace DASALUD.ViewModels
{
    public class CreateCitaViewModel
    {
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

        public int IdEstado { get; set; }

        // Listas para dropdowns
        public List<PacienteSelectItem>? Pacientes { get; set; }
        public List<EmpleadoSelectItem>? Empleados { get; set; }
    }

    public class PacienteSelectItem
    {
        public int IdPaciente { get; set; }
        public string NombreCompleto { get; set; } = "";
    }

    public class EmpleadoSelectItem
    {
        public int IdEmpleado { get; set; }
        public string NombreCompleto { get; set; } = "";
        public string Especialidad { get; set; } = "";
    }
}
