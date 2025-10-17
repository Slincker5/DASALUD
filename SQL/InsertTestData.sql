/* =========================================================
   DATOS DE PRUEBA PARA EL DASHBOARD
   Ejecuta este script DESPUÉS de crear la base de datos
   ========================================================= */

USE [dasalud-core-db];
GO

-- Insertar pacientes de prueba
DECLARE @idPersona1 INT, @idPersona2 INT, @idPersona3 INT, @idPersona4 INT, @idPersona5 INT;

INSERT INTO PERSONAS (nombres, apellidos, correo, DUI, telefono, direccion)
VALUES ('Juan', 'Pérez', 'juan.perez@example.com', '12345678-9', '7777-7777', 'San Salvador');
SET @idPersona1 = SCOPE_IDENTITY();

INSERT INTO PERSONAS (nombres, apellidos, correo, DUI, telefono, direccion)
VALUES ('María', 'García', 'maria.garcia@example.com', '98765432-1', '7777-8888', 'Santa Ana');
SET @idPersona2 = SCOPE_IDENTITY();

INSERT INTO PERSONAS (nombres, apellidos, correo, DUI, telefono, direccion)
VALUES ('Carlos', 'López', 'carlos.lopez@example.com', '11111111-1', '7777-9999', 'San Miguel');
SET @idPersona3 = SCOPE_IDENTITY();

INSERT INTO PERSONAS (nombres, apellidos, correo, DUI, telefono, direccion)
VALUES ('Ana', 'Martínez', 'ana.martinez@example.com', '22222222-2', '7777-6666', 'La Libertad');
SET @idPersona4 = SCOPE_IDENTITY();

INSERT INTO PERSONAS (nombres, apellidos, correo, DUI, telefono, direccion)
VALUES ('Pedro', 'Hernández', 'pedro.hernandez@example.com', '33333333-3', '7777-5555', 'Usulután');
SET @idPersona5 = SCOPE_IDENTITY();

-- Crear registros de pacientes
INSERT INTO PACIENTES (idPaciente, tipoSangre, peso, altura, activo)
VALUES 
(@idPersona1, 'O+', 70.5, 1.75, 1),
(@idPersona2, 'A+', 65.0, 1.65, 1),
(@idPersona3, 'B+', 80.0, 1.80, 1),
(@idPersona4, 'AB+', 55.5, 1.60, 1),
(@idPersona5, 'O-', 75.0, 1.70, 1);

-- Insertar médico adicional de prueba
DECLARE @idPersonaMedico INT;
INSERT INTO PERSONAS (nombres, apellidos, correo, DUI, telefono, direccion)
VALUES ('Dr. Roberto', 'Sánchez', 'roberto.sanchez@dasalud.com', '44444444-4', '7777-4444', 'San Salvador');
SET @idPersonaMedico = SCOPE_IDENTITY();

INSERT INTO EMPLEADOS (idEmpleado, idRol, idEspecialidad, usuario, contrasena, activo)
VALUES (@idPersonaMedico, 2, 2, 'dr.sanchez', 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=', 1);
-- Contraseña: admin123

-- Insertar citas de diferentes meses y especialidades
DECLARE @hoy DATETIME = GETDATE();

-- Citas completadas (últimos 6 meses)
-- Enero
INSERT INTO CITAS (idPaciente, idEmpleado, idEstado, fechaCita, costo, activo)
VALUES 
(@idPersona1, 1, 4, DATEADD(MONTH, -5, @hoy), 50.00, 1),
(@idPersona2, 1, 4, DATEADD(MONTH, -5, @hoy), 50.00, 1),
(@idPersona3, @idPersonaMedico, 4, DATEADD(MONTH, -5, @hoy), 75.00, 1);

-- Febrero
INSERT INTO CITAS (idPaciente, idEmpleado, idEstado, fechaCita, costo, activo)
VALUES 
(@idPersona1, 1, 4, DATEADD(MONTH, -4, @hoy), 50.00, 1),
(@idPersona2, @idPersonaMedico, 4, DATEADD(MONTH, -4, @hoy), 75.00, 1),
(@idPersona3, 1, 4, DATEADD(MONTH, -4, @hoy), 50.00, 1),
(@idPersona4, @idPersonaMedico, 4, DATEADD(MONTH, -4, @hoy), 75.00, 1),
(@idPersona5, 1, 4, DATEADD(MONTH, -4, @hoy), 50.00, 1);

-- Marzo
INSERT INTO CITAS (idPaciente, idEmpleado, idEstado, fechaCita, costo, activo)
VALUES 
(@idPersona1, 1, 4, DATEADD(MONTH, -3, @hoy), 50.00, 1),
(@idPersona2, 1, 4, DATEADD(MONTH, -3, @hoy), 50.00, 1),
(@idPersona3, @idPersonaMedico, 4, DATEADD(MONTH, -3, @hoy), 75.00, 1),
(@idPersona4, 1, 4, DATEADD(MONTH, -3, @hoy), 50.00, 1);

-- Abril
INSERT INTO CITAS (idPaciente, idEmpleado, idEstado, fechaCita, costo, activo)
VALUES 
(@idPersona1, @idPersonaMedico, 4, DATEADD(MONTH, -2, @hoy), 75.00, 1),
(@idPersona2, 1, 4, DATEADD(MONTH, -2, @hoy), 50.00, 1),
(@idPersona3, @idPersonaMedico, 4, DATEADD(MONTH, -2, @hoy), 75.00, 1),
(@idPersona4, 1, 4, DATEADD(MONTH, -2, @hoy), 50.00, 1),
(@idPersona5, @idPersonaMedico, 4, DATEADD(MONTH, -2, @hoy), 75.00, 1),
(@idPersona1, 1, 4, DATEADD(MONTH, -2, @hoy), 50.00, 1);

-- Mayo
INSERT INTO CITAS (idPaciente, idEmpleado, idEstado, fechaCita, costo, activo)
VALUES 
(@idPersona2, @idPersonaMedico, 4, DATEADD(MONTH, -1, @hoy), 75.00, 1),
(@idPersona3, 1, 4, DATEADD(MONTH, -1, @hoy), 50.00, 1),
(@idPersona4, @idPersonaMedico, 4, DATEADD(MONTH, -1, @hoy), 75.00, 1),
(@idPersona5, 1, 4, DATEADD(MONTH, -1, @hoy), 50.00, 1),
(@idPersona1, @idPersonaMedico, 4, DATEADD(MONTH, -1, @hoy), 75.00, 1);

-- Junio (mes actual) - Citas completadas y pendientes
INSERT INTO CITAS (idPaciente, idEmpleado, idEstado, fechaCita, costo, activo)
VALUES 
(@idPersona1, 1, 4, DATEADD(DAY, -15, @hoy), 50.00, 1),
(@idPersona2, @idPersonaMedico, 4, DATEADD(DAY, -10, @hoy), 75.00, 1),
(@idPersona3, 1, 4, DATEADD(DAY, -8, @hoy), 50.00, 1),
(@idPersona4, @idPersonaMedico, 4, DATEADD(DAY, -5, @hoy), 75.00, 1),
(@idPersona5, 1, 4, DATEADD(DAY, -3, @hoy), 50.00, 1),
(@idPersona1, @idPersonaMedico, 4, DATEADD(DAY, -1, @hoy), 75.00, 1);

-- Citas pendientes (futuras)
INSERT INTO CITAS (idPaciente, idEmpleado, idEstado, fechaCita, costo, activo)
VALUES 
(@idPersona1, 1, 1, DATEADD(DAY, 2, @hoy), 50.00, 1),
(@idPersona2, @idPersonaMedico, 2, DATEADD(DAY, 3, @hoy), 75.00, 1),
(@idPersona3, 1, 1, DATEADD(DAY, 5, @hoy), 50.00, 1),
(@idPersona4, @idPersonaMedico, 2, DATEADD(DAY, 7, @hoy), 75.00, 1),
(@idPersona5, 1, 1, DATEADD(DAY, 10, @hoy), 50.00, 1);

GO

PRINT '? Datos de prueba insertados correctamente.';
PRINT '';
PRINT '?? Resumen:';
PRINT '   - 5 Pacientes';
PRINT '   - 1 Médico adicional (dr.sanchez / admin123)';
PRINT '   - ~30 Citas completadas (últimos 6 meses)';
PRINT '   - 5 Citas pendientes';
PRINT '';
PRINT '?? Ahora puedes ver las gráficas con datos en el dashboard.';
GO
