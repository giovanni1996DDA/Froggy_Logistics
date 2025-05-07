CREATE DATABASE Services;
GO
USE Services;
GO

-- Tabla: Permiso
CREATE TABLE Permiso (
    ID_Permiso UNIQUEIDENTIFIER PRIMARY KEY,
    Codigo_Permiso INT IDENTITY(1,1) UNIQUE,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(200),
    Form NVARCHAR(100)
);

-- Tabla: Rol
CREATE TABLE Rol (
    ID_Rol UNIQUEIDENTIFIER PRIMARY KEY,
    Codigo_Rol INT IDENTITY(1,1) UNIQUE,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(200)
);

-- Tabla: RolRolRelation
CREATE TABLE RolRolRelation (
    ID_Rol_Padre UNIQUEIDENTIFIER,
    ID_Rol_Hijo UNIQUEIDENTIFIER,
    PRIMARY KEY (ID_Rol_Padre, ID_Rol_Hijo),
    FOREIGN KEY (ID_Rol_Padre) REFERENCES Rol(ID_Rol),
    FOREIGN KEY (ID_Rol_Hijo) REFERENCES Rol(ID_Rol)
);

-- Tabla: RolPermisoRelation
CREATE TABLE RolPermisoRelation (
    ID_Rol UNIQUEIDENTIFIER,
    ID_Permiso UNIQUEIDENTIFIER,
    PRIMARY KEY (ID_Rol, ID_Permiso),
    FOREIGN KEY (ID_Rol) REFERENCES Rol(ID_Rol),
    FOREIGN KEY (ID_Permiso) REFERENCES Permiso(ID_Permiso)
);

-- Tabla: TipoDocumento
CREATE TABLE DocType (
    ID_DocType INT PRIMARY KEY,
    Nombre NVARCHAR(50)
);

-- Crear tabla de estados de usuario
CREATE TABLE UserStatus (
    ID_Status INT PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,
    Descripcion NVARCHAR(200)
);

CREATE TABLE AppUser (
    ID_User UNIQUEIDENTIFIER PRIMARY KEY,
    Legajo INT IDENTITY(1,1) UNIQUE,
    Username NVARCHAR(255) NOT NULL,
    Password NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100),
    Nombre NVARCHAR(100),
    Apellido NVARCHAR(100),
    ID_Status INT NOT NULL,
    ID_DocType INT,
    NumeroDocumento NVARCHAR(50),
    FOREIGN KEY (ID_DocType) REFERENCES DocType(ID_DocType),
    FOREIGN KEY (ID_Status) REFERENCES UserStatus(ID_Status)
);

-- Índice único fuera del bloque CREATE TABLE
CREATE UNIQUE INDEX IX_User_Username ON AppUser (Username);

-- Tabla: UsuarioRolRelation
CREATE TABLE UserRolRelation (
    ID_User UNIQUEIDENTIFIER,
    ID_Rol UNIQUEIDENTIFIER,
    PRIMARY KEY (ID_User, ID_Rol),
    FOREIGN KEY (ID_User) REFERENCES AppUser(ID_User),
    FOREIGN KEY (ID_Rol) REFERENCES Rol(ID_Rol)
);


-- Tabla: UsuarioPermisoRelation
CREATE TABLE UserPermisoRelation (
    ID_User UNIQUEIDENTIFIER,
    ID_Permiso UNIQUEIDENTIFIER,
    PRIMARY KEY (ID_User, ID_Permiso),
    FOREIGN KEY (ID_User) REFERENCES AppUser(ID_User),
    FOREIGN KEY (ID_Permiso) REFERENCES Permiso(ID_Permiso)
);
