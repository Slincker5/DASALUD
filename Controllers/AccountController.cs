using DASALUD.Data;
using DASALUD.Helpers;
using DASALUD.Models;
using DASALUD.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DASALUD.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = PasswordHelper.HashPassword(model.Password);

                var empleado = await _context.Empleados
                    .Include(e => e.Persona)
                    .Include(e => e.Rol)
                    .Include(e => e.Especialidad)
                    .FirstOrDefaultAsync(e => 
                        e.Usuario == model.Username 
                        && e.Contrasena == hashedPassword 
                        && e.Activo);

                if (empleado != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, empleado.IdEmpleado.ToString()),
                        new Claim(ClaimTypes.Name, empleado.Usuario),
                        new Claim(ClaimTypes.Email, empleado.Persona.Correo ?? ""),
                        new Claim(ClaimTypes.GivenName, empleado.Persona.Nombres),
                        new Claim(ClaimTypes.Surname, empleado.Persona.Apellidos),
                        new Claim(ClaimTypes.Role, empleado.Rol.NombreRol),
                        new Claim("Especialidad", empleado.Especialidad.NombreEspecialidad),
                        new Claim("IdRol", empleado.IdRol.ToString()),
                        new Claim("IdEspecialidad", empleado.IdEspecialidad.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                    return View(model);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
