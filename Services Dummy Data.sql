
USE Services;
GO

-- Insertar tipos de documento
INSERT INTO TipoDocumento (ID_TipoDocumento, Nombre) VALUES (1, 'DNI');

-- Insertar un usuario
INSERT INTO Usuario (ID_Usuario, Usuario, Contraseña, Email, Nombre, ID_TipoDocumento, NumeroDocumento)
VALUES ('383287fb-4020-4a46-bd7f-6ec3faa9bf00', 'admin', 'admin123', 'admin@empresa.com', 'Administrador General', 1, '12345678');

-- Insertar un permiso (parte de un rol)
INSERT INTO Permiso (ID_Permiso, Nombre, Descripcion, Formulario)
VALUES ('e963de1d-db3a-48f0-8d28-9a0115084a48', 'Ver Compras', 'Permite acceder al módulo de compras', 'FormCompras');

-- Insertar un permiso suelto
INSERT INTO Permiso (ID_Permiso, Nombre, Descripcion, Formulario)
VALUES ('24932606-e154-4368-b8dd-4f5b011b889a', 'Aprobar OC', 'Permite aprobar órdenes de compra', 'FormAprobarOC');

-- Insertar un rol
INSERT INTO Rol (ID_Rol, Nombre, Descripcion)
VALUES ('f396bdfa-3b16-47a0-9941-47016c5e8fca', 'Comprador', 'Rol con permisos de gestión de compras');

-- Asociar permiso al rol
INSERT INTO RolPermisoRelation (ID_Rol, ID_Permiso)
VALUES ('f396bdfa-3b16-47a0-9941-47016c5e8fca', 'e963de1d-db3a-48f0-8d28-9a0115084a48');

-- Asociar el rol al usuario
INSERT INTO UsuarioRolRelation (ID_Usuario, ID_Rol)
VALUES ('383287fb-4020-4a46-bd7f-6ec3faa9bf00', 'f396bdfa-3b16-47a0-9941-47016c5e8fca');

-- Asociar el permiso suelto directamente al usuario
INSERT INTO UsuarioPermisoRelation (ID_Usuario, ID_Permiso)
VALUES ('383287fb-4020-4a46-bd7f-6ec3faa9bf00', '24932606-e154-4368-b8dd-4f5b011b889a');
