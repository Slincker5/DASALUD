namespace DASALUD.ViewModels
{
    public class CitasViewModel
    {
        public IEnumerable<CitaRowViewModel> Citas { get; set; } = Enumerable.Empty<CitaRowViewModel>();
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public string? Query { get; set; } 
    }
    public class CitaRowViewModel
    {
        public string IdConsulta { get; set; } = "";
        public string AtendidoPor { get; set; } = "";
        public string Paciente { get; set; } = "";
        public DateTime Fecha { get; set; }
        public decimal Costo { get; set; }
        public string Estado { get; set; } = ""; // "Finalizada", "Pendiente", "Reprogramada"
        public int Id { get; set; } // PK para rutas Edit/Delete/Details
    }

}
