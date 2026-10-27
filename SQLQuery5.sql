USE WebApp2DB;
GO

CREATE OR ALTER PROCEDURE sp_Usuarios_ObtenerPorId
    @PKUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.PKUsuario, u.Nombre, u.Username, u.Password, u.Estatus, u.FKRol,
           r.Nombre AS RolNombre
    FROM Usuarios u
    LEFT JOIN Roles r ON r.PKRol = u.FKRol
    WHERE u.PKUsuario = @PKUsuario;
END
GO