USE WebApp2DB;
GO

CREATE OR ALTER PROCEDURE sp_Roles_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT PKRol, Nombre, Estatus FROM Roles;
END
GO

CREATE OR ALTER PROCEDURE sp_Roles_ObtenerPorId
    @PKRol INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT PKRol, Nombre, Estatus FROM Roles WHERE PKRol = @PKRol;
END
GO

CREATE OR ALTER PROCEDURE sp_Roles_Crear
    @Nombre NVARCHAR(MAX),
    @Estatus BIT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Roles (Nombre, Estatus) VALUES (@Nombre, @Estatus);

    SELECT PKRol, Nombre, Estatus FROM Roles WHERE PKRol = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_Roles_Actualizar
    @PKRol INT,
    @Nombre NVARCHAR(MAX),
    @Estatus BIT
AS
BEGIN
    UPDATE Roles SET Nombre = @Nombre, Estatus = @Estatus WHERE PKRol = @PKRol;
END
GO

CREATE OR ALTER PROCEDURE sp_Roles_Eliminar
    @PKRol INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Usuarios WHERE FKRol = @PKRol)
        THROW 50002, 'No se puede eliminar: hay usuarios con este rol.', 1;

    DELETE FROM Roles WHERE PKRol = @PKRol;
END
GO

CREATE OR ALTER PROCEDURE sp_Usuarios_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.PKUsuario, u.Nombre, u.Username, u.Estatus, u.FKRol,
           r.Nombre AS RolNombre
    FROM Usuarios u
    LEFT JOIN Roles r ON r.PKRol = u.FKRol;
END
GO

CREATE OR ALTER PROCEDURE sp_Usuarios_ObtenerPorId
    @PKUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.PKUsuario, u.Nombre, u.Username, u.Estatus, u.FKRol,
           r.Nombre AS RolNombre
    FROM Usuarios u
    LEFT JOIN Roles r ON r.PKRol = u.FKRol
    WHERE u.PKUsuario = @PKUsuario;
END
GO

CREATE OR ALTER PROCEDURE sp_Usuarios_Crear
    @Nombre NVARCHAR(MAX),
    @Username NVARCHAR(MAX),
    @Password NVARCHAR(MAX),
    @Estatus BIT,
    @FKRol INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Roles WHERE PKRol = @FKRol)
        THROW 50001, 'El rol indicado no existe. Crea el rol primero.', 1;

    INSERT INTO Usuarios (Nombre, Username, Password, Estatus, FKRol)
    VALUES (@Nombre, @Username, @Password, @Estatus, @FKRol);

    DECLARE @NuevoId INT = SCOPE_IDENTITY();

    SELECT u.PKUsuario, u.Nombre, u.Username, u.Estatus, u.FKRol,
           r.Nombre AS RolNombre
    FROM Usuarios u
    LEFT JOIN Roles r ON r.PKRol = u.FKRol
    WHERE u.PKUsuario = @NuevoId;
END
GO

CREATE OR ALTER PROCEDURE sp_Usuarios_Actualizar
    @PKUsuario INT,
    @Nombre NVARCHAR(MAX),
    @Username NVARCHAR(MAX),
    @Password NVARCHAR(MAX) = NULL,
    @Estatus BIT,
    @FKRol INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Roles WHERE PKRol = @FKRol)
        THROW 50001, 'El rol indicado no existe. Crea el rol primero.', 1;

    UPDATE Usuarios
    SET Nombre   = @Nombre,
        Username = @Username,
        Password = CASE WHEN @Password IS NULL OR @Password = '' THEN Password ELSE @Password END,
        Estatus  = @Estatus,
        FKRol    = @FKRol
    WHERE PKUsuario = @PKUsuario;
END
GO

CREATE OR ALTER PROCEDURE sp_Usuarios_Eliminar
    @PKUsuario INT
AS
BEGIN
    DELETE FROM Usuarios WHERE PKUsuario = @PKUsuario;
END
GO