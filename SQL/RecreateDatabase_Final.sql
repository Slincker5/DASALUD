/* =========================================================
   SCRIPT COMPLETO - DASALUD CORE DB
   Versión Final - Login por usuario y contraseña de EMPLEADOS
   ========================================================= */

/* =========================================================
   1. ELIMINAR Y RECREAR BASE DE DATOS
   ========================================================= */
USE master;
GO

IF DB_ID(N'dasalud-core-db') IS NOT NULL
BEGIN
    ALTER DATABASE [dasalud-core-db] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [dasalud-core-db];
END
GO

CREATE DATABASE [dasalud-core-db];
GO

USE [dasalud-core-db];
GO

/* =========================================================
   2. CREACIÓN DE TABLAS MAESTRAS
   ========================================================= */

-- PERSONAS (super-tipo)
CREATE TABLE dbo.PERSONAS
(
    idPersona       INT IDENTITY(1,1) NOT NULL,
    nombres         NVARCHAR(100)     NOT NULL,
    apellidos       NVARCHAR(100)     NOT NULL,
    correo          NVARCHAR(255)     NULL,
    DUI             VARCHAR(20)       NULL,
    telefono        VARCHAR(30)       NULL,
    direccion       NVARCHAR(255)     NULL,

    CONSTRAINT PK_PERSONAS PRIMARY KEY CLUSTERED (idPersona)
);
GO

-- ROLES
CREATE TABLE dbo.ROLES
(
    idRol           INT IDENTITY(1,1) NOT NULL,
    nombreRol       NVARCHAR(100)     NOT NULL,
    activo          BIT               NOT NULL CONSTRAINT DF_ROLES_activo DEFAULT (1),

    CONSTRAINT PK_ROLES PRIMARY KEY CLUSTERED (idRol),
    CONSTRAINT UQ_ROLES_nombre UNIQUE (nombreRol)
);
GO

-- ESPECIALIDADES
CREATE TABLE dbo.ESPECIALIDADES
(
    idEspecialidad      INT IDENTITY(1,1) NOT NULL,
    nombreEspecialidad  NVARCHAR(150)     NOT NULL,
    activo              BIT               NOT NULL CONSTRAINT DF_ESP_activo DEFAULT (1),

    CONSTRAINT PK_ESPECIALIDADES PRIMARY KEY CLUSTERED (idEspecialidad),
    CONSTRAINT UQ_ESPECIALIDADES_nombre UNIQUE (nombreEspecialidad)
);
GO

-- ESTADOS_CITAS
CREATE TABLE dbo.ESTADOS_CITAS
(
    idEstado        INT IDENTITY(1,1) NOT NULL,
    nombreEstado    NVARCHAR(100)     NOT NULL,
    activo          BIT               NOT NULL CONSTRAINT DF_ESTADOS_activo DEFAULT (1),

    CONSTRAINT PK_ESTADOS_CITAS PRIMARY KEY CLUSTERED (idEstado),
    CONSTRAINT UQ_ESTADOS_CITAS_nombre UNIQUE (nombreEstado)
);
GO

/* =========================================================
   3. CREACIÓN DE SUBTIPOS DE PERSONAS
   ========================================================= */

-- PACIENTES (PK = FK a PERSONAS)
CREATE TABLE dbo.PACIENTES
(
    idPaciente  INT             NOT NULL,  -- hereda de PERSONAS.idPersona
    tipoSangre  NVARCHAR(5)     NULL,
    peso        DECIMAL(6,2)    NULL,
    altura      DECIMAL(5,2)    NULL,
    activo      BIT             NOT NULL CONSTRAINT DF_PACIENTES_activo DEFAULT (1),

    CONSTRAINT PK_PACIENTES PRIMARY KEY CLUSTERED (idPaciente),
    CONSTRAINT FK_PACIENTES_PERSONAS
        FOREIGN KEY (idPaciente) REFERENCES dbo.PERSONAS(idPersona)
        ON DELETE CASCADE ON UPDATE NO ACTION,

    CONSTRAINT CK_PACIENTES_peso   CHECK (peso   IS NULL OR peso   >= 0),
    CONSTRAINT CK_PACIENTES_altura CHECK (altura IS NULL OR altura >= 0)
);
GO

-- EMPLEADOS (PK = FK a PERSONAS) - CON CAMPOS DE AUTENTICACIÓN
CREATE TABLE dbo.EMPLEADOS
(
    idEmpleado      INT             NOT NULL,  -- hereda de PERSONAS.idPersona
    idRol           INT             NOT NULL,
    idEspecialidad  INT             NOT NULL,
    usuario         NVARCHAR(255)   NOT NULL,
    contrasena      NVARCHAR(255)   NOT NULL,
    activo          BIT             NOT NULL CONSTRAINT DF_EMPLEADOS_activo DEFAULT (1),

    CONSTRAINT PK_EMPLEADOS PRIMARY KEY CLUSTERED (idEmpleado),

    CONSTRAINT FK_EMPLEADOS_PERSONAS
        FOREIGN KEY (idEmpleado) REFERENCES dbo.PERSONAS(idPersona)
        ON DELETE CASCADE ON UPDATE NO ACTION,

    CONSTRAINT FK_EMPLEADOS_ROLES
        FOREIGN KEY (idRol) REFERENCES dbo.ROLES(idRol),

    CONSTRAINT FK_EMPLEADOS_ESPECIALIDADES
        FOREIGN KEY (idEspecialidad) REFERENCES dbo.ESPECIALIDADES(idEspecialidad),

    CONSTRAINT UQ_EMPLEADOS_usuario UNIQUE (usuario)
);
GO

/* =========================================================
   4. CREACIÓN DE TABLAS TRANSACCIONALES
   ========================================================= */

-- CITAS
CREATE TABLE dbo.CITAS
(
    idCita      INT IDENTITY(1,1) NOT NULL,
    idPaciente  INT               NOT NULL,
    idEmpleado  INT               NOT NULL,
    idEstado    INT               NOT NULL,
    fechaCita   DATETIME2(0)      NOT NULL,
    costo       DECIMAL(12,2)     NOT NULL CONSTRAINT DF_CITAS_costo  DEFAULT (0),
    activo      BIT               NOT NULL CONSTRAINT DF_CITAS_activo DEFAULT (1),

    CONSTRAINT PK_CITAS PRIMARY KEY CLUSTERED (idCita),

    CONSTRAINT FK_CITAS_PACIENTES
        FOREIGN KEY (idPaciente) REFERENCES dbo.PACIENTES(idPaciente),

    CONSTRAINT FK_CITAS_EMPLEADOS
        FOREIGN KEY (idEmpleado) REFERENCES dbo.EMPLEADOS(idEmpleado),

    CONSTRAINT FK_CITAS_ESTADOS
        FOREIGN KEY (idEstado)   REFERENCES dbo.ESTADOS_CITAS(idEstado),

    CONSTRAINT CK_CITAS_costo CHECK (costo >= 0)
);
GO

/* =========================================================
   5. ÍNDICES ADICIONALES
   ========================================================= */
CREATE UNIQUE INDEX IXU_PERSONAS_correo
    ON dbo.PERSONAS(correo) WHERE correo IS NOT NULL;

CREATE UNIQUE INDEX IXU_PERSONAS_DUI
    ON dbo.PERSONAS(DUI) WHERE DUI IS NOT NULL;

CREATE INDEX IX_CITAS_fechaCita ON dbo.CITAS(fechaCita);
GO

/* =========================================================
   6. DATOS INICIALES DE CONFIGURACIÓN
   ========================================================= */

-- ROLES
INSERT INTO dbo.ROLES (nombreRol, activo) VALUES 
('Administrador', 1),
('Médico', 1),
('Enfermero', 1),
('Recepcionista', 1);
GO

-- ESPECIALIDADES
INSERT INTO dbo.ESPECIALIDADES (nombreEspecialidad, activo) VALUES 
('Medicina General', 1),
('Pediatría', 1),
('Cardiología', 1),
('Traumatología', 1),
('Administración', 1);
GO

-- ESTADOS_CITAS
INSERT INTO dbo.ESTADOS_CITAS (nombreEstado, activo) VALUES 
('Pendiente', 1),
('Confirmada', 1),
('En Progreso', 1),
('Completada', 1),
('Cancelada', 1);
GO

-- PERSONA ADMINISTRADOR
INSERT INTO dbo.PERSONAS (nombres, apellidos, correo, DUI, telefono, direccion) VALUES 
('Admin', 'Sistema', 'admin@dasalud.com', '00000000-0', '0000-0000', 'Oficina Central');
GO

-- EMPLEADO ADMINISTRADOR
-- Usuario: admin
-- Contraseña: admin123 (SHA256 + Base64)
INSERT INTO dbo.EMPLEADOS (idEmpleado, idRol, idEspecialidad, usuario, contrasena, activo)
VALUES (1, 1, 5, 'admin', 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=', 1);
GO

/* =========================================================
   7. VALIDACIÓN
   ========================================================= */
PRINT '? Base de datos [dasalud-core-db] creada correctamente.';
PRINT '? Datos iniciales insertados.';
PRINT '? Usuario de prueba creado:';
PRINT '   - Usuario: admin';
PRINT '   - Contraseña: admin123';
GO
