# 🏥 Gestión de Pacientes DASALUD

Sistema desarrollado en **ASP.NET Core MVC**, orientado a la administración integral de pacientes, citas y personal médico.  
Su arquitectura **Modelo–Vista–Controlador (MVC)** garantiza una separación lógica entre la interfaz, el control del flujo y la manipulación de datos, mejorando la **mantenibilidad**, **modularidad** y **seguridad** del sistema.

---

## ⚙️ Estructura Lógica General

### 🧩 Componentes MVC

| Componente | Descripción |
|-------------|-------------|
| **Modelo (Model)** | Define la estructura y reglas de negocio. Incluye las clases `Paciente`, `Empleado`, `Cita`, `Rol`, `Especialidad` y `EstadoCita`. Se relacionan mediante herencia y asociaciones uno-a-muchos. Utiliza **Entity Framework Core** para la persistencia de datos. |
| **Vista (View)** | Representa la interfaz visual, construida con **Razor Pages** o **Blazor Server**. Presenta formularios, listados y reportes interactivos que se comunican con los controladores. |
| **Controlador (Controller)** | Gestiona el flujo entre vistas y modelos. Procesa solicitudes, ejecuta lógica de negocio y devuelve respuestas o vistas. Ejemplos: `CitasController`, `PacientesController`, `EmpleadosController`, `AuthController`. |

---

## 🔄 Procesos Clave

- **Gestión de Citas Médicas** → Creación, edición y validación de disponibilidad.  
- **Gestión de Pacientes** → Registro y actualización de información personal.  
- **Gestión de Empleados** → Administración del personal médico y roles.  
- **Autenticación y Control de Acceso** → Inicio de sesión y permisos por rol.  
- **Reportería y Análisis** → Generación de indicadores y gráficos sobre actividad médica.

---

## 🔁 Flujo de Datos en el MVC

1. El usuario realiza una acción en la **vista** (por ejemplo, registrar una cita).  
2. La vista envía la solicitud **HTTP** al **controlador** correspondiente.  
3. El controlador ejecuta la lógica necesaria e invoca métodos del **modelo**.  
4. El modelo interactúa con la **base de datos** mediante **Entity Framework Core**.  
5. Los resultados se devuelven al **controlador**, que los presenta nuevamente en la vista.

---

## 🧠 Interacción de Componentes

- Las **vistas** solo muestran información, sin acceder directamente a la base de datos.  
- Los **controladores** actúan como intermediarios entre la interfaz y los datos.  
- Los **modelos** representan la lógica interna y las reglas del sistema.  
- Los **servicios** centralizan procesos complejos, evitando lógica duplicada.

---

## 🧾 Licencia

Proyecto bajo licencia **[Creative Commons Zero v1.0 Universal (CC0 1.0)](https://creativecommons.org/publicdomain/zero/1.0/)**. Puedes usarlo, modificarlo y distribuirlo libremente, siempre que se mantenga el aviso de copyright original.

---

**Tecnología base:** ASP.NET Core 8 · Entity Framework Core · Razor / Blazor Server / MVC
