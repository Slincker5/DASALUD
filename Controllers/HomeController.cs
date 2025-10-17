using System.Diagnostics;
using DASALUD.Data;
using DASALUD.Models;
using DASALUD.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DASALUD.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel();

            viewModel.PacientesAtendidos = await _context.Citas
                .Where(c => c.Estado.NombreEstado == "Completada" && c.Activo)
                .Select(c => c.IdPaciente)
                .Distinct()
                .CountAsync();

            viewModel.ConsultasPendientes = await _context.Citas
                .Where(c => (c.Estado.NombreEstado == "Pendiente" || c.Estado.NombreEstado == "Confirmada") 
                    && c.Activo)
                .CountAsync();

            viewModel.TotalConsultas = await _context.Citas
                .Where(c => c.Activo)
                .CountAsync();

            var sixMonthsAgo = DateTime.Now.AddMonths(-6);
            var consultasPorMes = await _context.Citas
                .Where(c => c.FechaCita >= sixMonthsAgo && c.Activo)
                .GroupBy(c => new { c.FechaCita.Year, c.FechaCita.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToListAsync();

            string[] meses = { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };
            viewModel.ConsultasPorMes = consultasPorMes.Select(c => new ConsultasPorMesData
            {
                Mes = meses[c.Month - 1],
                Cantidad = c.Count
            }).ToList();

            var consultasPorEsp = await _context.Citas
                .Include(c => c.Empleado.Especialidad)
                .Where(c => c.Activo)
                .GroupBy(c => c.Empleado.Especialidad)
                .Select(g => new ConsultasPorEspecialidadData
                {
                    Especialidad = g.Key.NombreEspecialidad,
                    Codigo = g.Key.NombreEspecialidad.Substring(0, 3).ToUpper(),
                    Cantidad = g.Count()
                })
                .OrderByDescending(x => x.Cantidad)
                .Take(6)
                .ToListAsync();

            viewModel.ConsultasPorEspecialidad = consultasPorEsp;

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
