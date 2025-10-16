# DASALUD - Sistema de Gestión de Salud

## 🔐 Credenciales de Prueba

Si ejecutaste el script SQL completo, tienes disponible:

- **Usuario**: `admin`
- **Contraseña**: `admin123`
- **Rol**: Administrador

## 🚀 Cómo Empezar

### 1. Configurar Base de Datos



### 2. Configurar Connection String

En `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "Default": "Server=tu_servidor;Database=dasalud-core-db;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```


Navega a `https://localhost:5001/Account/Login`

## 📝 Cómo Crear Nuevos Usuarios

### Usando SQL

```sql
-- 1. Crear persona
INSERT INTO PERSONAS (nombres, apellidos, correo, DUI, telefono, direccion)
VALUES ('María', 'García', 'maria@dasalud.com', '12345678-9', '7777-8888', 'San Salvador');

-- 2. Obtener ID
DECLARE @idPersona INT = SCOPE_IDENTITY();

-- 3. Hash de contraseña (usa PasswordHashGenerator o calcula manualmente)
-- Ejemplo: hash de 'maria123'
DECLARE @password NVARCHAR(255) = 'aquí_va_el_hash';

-- 4. Crear empleado
INSERT INTO EMPLEADOS (idEmpleado, idRol, idEspecialidad, usuario, contrasena, activo)
VALUES (@idPersona, 2, 1, 'maria.garcia', @password, 1);
```

### Generar Hash de Contraseña

Usa la herramienta `PasswordHashGenerator.cs` o en C#:

```csharp
using DASALUD.Helpers;

string password = "micontraseña123";
string hash = PasswordHelper.HashPassword(password);
Console.WriteLine(hash);
```

## 🔒 Claims Disponibles

Después del login, estos claims están disponibles:

- `ClaimTypes.NameIdentifier` - ID del empleado
- `ClaimTypes.Name` - Usuario
- `ClaimTypes.Email` - Correo electrónico
- `ClaimTypes.GivenName` - Nombres
- `ClaimTypes.Surname` - Apellidos
- `ClaimTypes.Role` - Nombre del rol
- `"Especialidad"` - Nombre de la especialidad
- `"IdRol"` - ID del rol
- `"IdEspecialidad"` - ID de la especialidad

### Usar en Controladores

```csharp
// Obtener ID del empleado
var empleadoId = User.FindFirstValue(ClaimTypes.NameIdentifier);

// Obtener rol
var rol = User.FindFirstValue(ClaimTypes.Role);

// Verificar autenticación
if (User.Identity.IsAuthenticated)
{
    // Usuario logueado
}
```

### Usar en Vistas

```razor
@if (User.Identity.IsAuthenticated)
{
    <p>Bienvenido, @User.Identity.Name</p>
    <p>Rol: @User.FindFirstValue(ClaimTypes.Role)</p>
}
```

## 🛡️ Autorización por Roles

Protege acciones o controladores:

```csharp
using Microsoft.AspNetCore.Authorization;

[Authorize] // Solo usuarios autenticados
public class MiController : Controller
{
    [Authorize(Roles = "Administrador")]
    public IActionResult AdminOnly()
    {
        return View();
    }

    [Authorize(Roles = "Administrador,Médico")]
    public IActionResult MedicoOAdmin()
    {
        return View();
    }
}
```

## 📊 Estructura de la Base de Datos

### Tablas Maestras
- **PERSONAS** - Datos básicos de personas
- **ROLES** - Roles del sistema
- **ESPECIALIDADES** - Especialidades médicas
- **ESTADOS_CITAS** - Estados de las citas

### Subtipos de Personas
- **PACIENTES** - Pacientes del sistema
- **EMPLEADOS** - Empleados con acceso al sistema

### Transaccionales
- **CITAS** - Citas médicas

## 🔄 Próximos Pasos Sugeridos

1. ✨ Implementar recuperación de contraseña
2. ✨ Agregar cambio de contraseña
3. ✨ Implementar registro de intentos fallidos
4. ✨ Agregar bloqueo de cuenta por seguridad
5. ✨ Mejorar hash con BCrypt (más seguro que SHA256)
6. ✨ Implementar auditoría de accesos
7. ✨ Agregar vista de registro de nuevos empleados

## 📚 Documentación Adicional

- Ver `MIGRATION_GUIDE.md` para detalles completos de migración
- Ver scripts SQL en carpeta `SQL/`
- Ver helpers en `Helpers/`

## ⚠️ Notas Importantes

1. **Seguridad**: El hash SHA256 es básico. Para producción, considera BCrypt:
   ```bash
   dotnet add package BCrypt.Net-Next
   ```

2. **Contraseñas**: Cambia las contraseñas de prueba en producción

3. **Connection String**: No expongas tu connection string en producción

4. **HTTPS**: Siempre usa HTTPS en producción

