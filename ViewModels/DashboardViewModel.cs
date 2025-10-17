namespace DASALUD.ViewModels
{
    public class DashboardViewModel
    {
        public int PacientesAtendidos { get; set; }
        public int ConsultasPendientes { get; set; }
        public int TotalConsultas { get; set; }
        public List<ConsultasPorMesData> ConsultasPorMes { get; set; } = new();
        public List<ConsultasPorEspecialidadData> ConsultasPorEspecialidad { get; set; } = new();
    }

    public class ConsultasPorMesData
    {
        public string Mes { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }

    public class ConsultasPorEspecialidadData
    {
        public string Especialidad { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }
}
