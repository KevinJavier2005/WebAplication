USE WebApp2DB;
SELECT name FROM sys.procedures WHERE name LIKE 'sp[_]Roles%';

EXEC sp_Roles_Crear @Nombre = 'Prueba', @Estatus = 1;
EXEC sp_Roles_Listar;
EXEC sp_Roles_ObtenerPorId @PKRol = 1;
EXEC sp_Roles_Actualizar @PKRol = 1, @Nombre = 'Prueba 2', @Estatus = 1;
EXEC sp_Roles_Eliminar @PKRol = 1;