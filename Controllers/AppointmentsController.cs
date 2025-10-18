using DASALUD.Data;
using DASALUD.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DASALUD.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AppointmentsController> _logger;

        public AppointmentsController(AppDbContext context, ILogger<AppointmentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? q = null)
        {
            var query = _context.Citas
                .Include(c => c.Paciente.Persona)
                .Include(c => c.Empleado.Persona)
                .Include(c => c.Estado)
                .Where(c => c.Activo)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(c =>
                    c.Paciente.Persona.Nombres.Contains(q) ||
                    c.Paciente.Persona.Apellidos.Contains(q) ||
                    c.Empleado.Persona.Nombres.Contains(q) ||
                    c.Empleado.Persona.Apellidos.Contains(q));
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var citas = await query
                .OrderByDescending(c => c.FechaCita)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CitaRowViewModel
                {
                    Id = c.IdCita,
                    IdConsulta = $"#{c.IdCita:D4}",
                    AtendidoPor = $"{c.Empleado.Persona.Nombres} {c.Empleado.Persona.Apellidos}",
                    Paciente = $"{c.Paciente.Persona.Nombres} {c.Paciente.Persona.Apellidos}",
                    Fecha = c.FechaCita,
                    Costo = c.Costo,
                    Estado = c.Estado.NombreEstado
                })
                .ToListAsync();

            var viewModel = new CitasViewModel
            {
                Citas = citas,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalItems = totalItems,
                Query = q
            };

            return View("Citas", viewModel);
        }

        // GET: Appointments/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateCitaViewModel
            {
                FechaCita = DateTime.Now,
                Costo = 29.00m
            };

            // Obtener lista de pacientes activos
            viewModel.Pacientes = await _context.Pacientes
                .Include(p => p.Persona)
                .Where(p => p.Activo)
                .OrderBy(p => p.Persona.Nombres)
                .ThenBy(p => p.Persona.Apellidos)
                .Select(p => new PacienteSelectItem
                {
                    IdPaciente = p.IdPaciente,
                    NombreCompleto = p.Persona.Nombres + " " + p.Persona.Apellidos
                })
                .ToListAsync();

            // Obtener lista de empleados (médicos) activos
            viewModel.Empleados = await _context.Empleados
                .Include(e => e.Persona)
                .Include(e => e.Especialidad)
                .Where(e => e.Activo && e.Rol.NombreRol == "Médico")
                .OrderBy(e => e.Persona.Nombres)
                .ThenBy(e => e.Persona.Apellidos)
                .Select(e => new EmpleadoSelectItem
                {
                    IdEmpleado = e.IdEmpleado,
                    NombreCompleto = e.Persona.Nombres + " " + e.Persona.Apellidos,
                    Especialidad = e.Especialidad.NombreEspecialidad
                })
                .ToListAsync();

            return View(viewModel);
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCitaViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Obtener el estado "Pendiente"
                    var estadoPendiente = await _context.EstadosCitas
                        .FirstOrDefaultAsync(e => e.NombreEstado == "Pendiente");

                    if (estadoPendiente == null)
                    {
                        TempData["Error"] = "No se encontró el estado 'Pendiente' en el sistema.";
                        return RedirectToAction(nameof(Create));
                    }

                    var cita = new Models.Cita
                    {
                        IdPaciente = model.IdPaciente,
                        IdEmpleado = model.IdEmpleado,
                        IdEstado = estadoPendiente.IdEstado,
                        FechaCita = model.FechaCita,
                        Costo = model.Costo,
                        Activo = true
                    };

                    _context.Add(cita);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = $"La cita fue creada exitosamente para el {model.FechaCita:dd/MM/yyyy}.";
                    _logger.LogInformation($"Cita creada por usuario {User.Identity?.Name}");

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al crear la cita");
                    TempData["Error"] = "Ocurrió un error al crear la cita. Por favor, intente nuevamente.";
                }
            }

            model.Pacientes = await _context.Pacientes
             .Where(p => p.Activo)
             .OrderBy(p => p.Persona.Nombres)
             .ThenBy(p => p.Persona.Apellidos)
             .Select(p => new PacienteSelectItem
             {
                 IdPaciente = p.IdPaciente,
                 NombreCompleto = p.Persona.Nombres + " " + p.Persona.Apellidos
             })
             .ToListAsync();

            model.Empleados = await _context.Empleados
            .Where(e => e.Activo && e.Rol.NombreRol == "Médico")
            .OrderBy(e => e.Persona.Nombres)
            .ThenBy(e => e.Persona.Apellidos)
            .Select(e => new EmpleadoSelectItem
            {
                IdEmpleado = e.IdEmpleado,
                NombreCompleto = e.Persona.Nombres + " " + e.Persona.Apellidos,
                Especialidad = e.Especialidad.NombreEspecialidad
            })
            .ToListAsync();

            return View(model);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas
                .Include(c => c.Paciente.Persona)
                .Include(c => c.Empleado.Persona)
                .Include(c => c.Estado)
                .FirstOrDefaultAsync(c => c.IdCita == id && c.Activo);

            if (cita == null)
            {
                return NotFound();
            }

            var viewModel = new EditCitaViewModel
            {
                IdCita = cita.IdCita,
                IdPaciente = cita.IdPaciente,
                IdEmpleado = cita.IdEmpleado,
                IdEstado = cita.IdEstado,
                Costo = cita.Costo,
                FechaCita = cita.FechaCita
            };

            // Obtener lista de pacientes activos
            viewModel.Pacientes = await _context.Pacientes
                .Include(p => p.Persona)
                .Where(p => p.Activo)
                .OrderBy(p => p.Persona.Nombres)
                .ThenBy(p => p.Persona.Apellidos)
                .Select(p => new PacienteSelectItem
                {
                    IdPaciente = p.IdPaciente,
                    NombreCompleto = p.Persona.Nombres + " " + p.Persona.Apellidos
                })
                .ToListAsync();

            // Obtener lista de empleados (médicos) activos
            viewModel.Empleados = await _context.Empleados
                .Include(e => e.Persona)
                .Include(e => e.Especialidad)
                .Where(e => e.Activo && e.Rol.NombreRol == "Médico")
                .OrderBy(e => e.Persona.Nombres)
                .ThenBy(e => e.Persona.Apellidos)
                .Select(e => new EmpleadoSelectItem
                {
                    IdEmpleado = e.IdEmpleado,
                    NombreCompleto = e.Persona.Nombres + " " + e.Persona.Apellidos,
                    Especialidad = e.Especialidad.NombreEspecialidad
                })
                .ToListAsync();

            // Obtener lista de estados de cita
            viewModel.Estados = await _context.EstadosCitas
                .OrderBy(e => e.NombreEstado)
                .Select(e => new EstadoCitaSelectItem
                {
                    IdEstado = e.IdEstado,
                    NombreEstado = e.NombreEstado
                })
                .ToListAsync();

            return View(viewModel);
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCitaViewModel model)
        {
            if (id != model.IdCita)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cita = await _context.Citas.FindAsync(id);

                    if (cita == null || !cita.Activo)
                    {
                        TempData["Error"] = "La cita no fue encontrada.";
                        return RedirectToAction(nameof(Index));
                    }

                    cita.IdPaciente = model.IdPaciente;
                    cita.IdEmpleado = model.IdEmpleado;
                    cita.IdEstado = model.IdEstado;
                    cita.FechaCita = model.FechaCita;
                    cita.Costo = model.Costo;

                    _context.Update(cita);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = $"La cita #{id:D4} fue actualizada exitosamente.";
                    _logger.LogInformation($"Cita {id} actualizada por usuario {User.Identity?.Name}");

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitaExists(model.IdCita))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error al actualizar la cita {id}");
                    TempData["Error"] = "Ocurrió un error al actualizar la cita. Por favor, intente nuevamente.";
                }
            }

            // Si hay errores, recargar las listas
            model.Pacientes = await _context.Pacientes
                .Include(p => p.Persona)
                .Where(p => p.Activo)
                .OrderBy(p => p.Persona.Nombres)
                .ThenBy(p => p.Persona.Apellidos)
                .Select(p => new PacienteSelectItem
                {
                    IdPaciente = p.IdPaciente,
                    NombreCompleto = p.Persona.Nombres + " " + p.Persona.Apellidos
                })
                .ToListAsync();

            model.Empleados = await _context.Empleados
                .Include(e => e.Persona)
                .Include(e => e.Especialidad)
                .Where(e => e.Activo && e.Rol.NombreRol == "Médico")
                .OrderBy(e => e.Persona.Nombres)
                .ThenBy(e => e.Persona.Apellidos)
                .Select(e => new EmpleadoSelectItem
                {
                    IdEmpleado = e.IdEmpleado,
                    NombreCompleto = e.Persona.Nombres + " " + e.Persona.Apellidos,
                    Especialidad = e.Especialidad.NombreEspecialidad
                })
                .ToListAsync();

            model.Estados = await _context.EstadosCitas
                .OrderBy(e => e.NombreEstado)
                .Select(e => new EstadoCitaSelectItem
                {
                    IdEstado = e.IdEstado,
                    NombreEstado = e.NombreEstado
                })
                .ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas
                .Include(c => c.Paciente.Persona)
                .Include(c => c.Empleado.Persona)
                .Include(c => c.Empleado.Especialidad)
                .Include(c => c.Estado)
                .FirstOrDefaultAsync(m => m.IdCita == id);

            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        // POST: Appointments/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var cita = await _context.Citas
                    .Include(c => c.Paciente.Persona)
                    .FirstOrDefaultAsync(c => c.IdCita == id && c.Activo);

                if (cita == null)
                {
                    TempData["Error"] = "La cita no fue encontrada.";
                    return RedirectToAction(nameof(Index));
                }

                // Soft delete - marca como inactivo
                cita.Activo = false;
                _context.Update(cita);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"La cita #{id:D4} fue eliminada exitosamente.";
                _logger.LogInformation($"Cita {id} eliminada por usuario {User.Identity?.Name}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar la cita {id}");
                TempData["Error"] = "Ocurrió un error al eliminar la cita. Por favor, intente nuevamente.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CitaExists(int id)
        {
            return _context.Citas.Any(e => e.IdCita == id);
        }
    }
}
