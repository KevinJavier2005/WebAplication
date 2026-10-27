using Dapper;
using Domain.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApplication2.Context;

namespace WebApplication2.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly DapperContext _context;

        public UsuarioService(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UsuarioResponseDto>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();

            return await connection.QueryAsync<UsuarioResponseDto>(
                "sp_Usuarios_Listar",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<UsuarioDetalleDto?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<UsuarioDetalleDto>(
                "sp_Usuarios_ObtenerPorId",
                new { PKUsuario = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<UsuarioResponseDto> CreateAsync(UsuarioCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new InvalidOperationException("La contraseña es obligatoria.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            try
            {
                using var connection = _context.CreateConnection();

                return await connection.QuerySingleAsync<UsuarioResponseDto>(
                    "sp_Usuarios_Crear",
                    new
                    {
                        dto.Nombre,
                        dto.Username,
                        Password = passwordHash,
                        dto.Estatus,
                        dto.FKRol
                    },
                    commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex) when (ex.Number == 50001)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<bool> UpdateAsync(int id, UsuarioUpdateDto dto)
        {
            string? passwordHash = string.IsNullOrWhiteSpace(dto.Password)
                ? null
                : BCrypt.Net.BCrypt.HashPassword(dto.Password);

            try
            {
                using var connection = _context.CreateConnection();

                var filas = await connection.ExecuteAsync(
                    "sp_Usuarios_Actualizar",
                    new
                    {
                        PKUsuario = id,
                        dto.Nombre,
                        dto.Username,
                        Password = passwordHash,
                        dto.Estatus,
                        dto.FKRol
                    },
                    commandType: CommandType.StoredProcedure);

                return filas > 0;
            }
            catch (SqlException ex) when (ex.Number == 50001)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();

            var filas = await connection.ExecuteAsync(
                "sp_Usuarios_Eliminar",
                new { PKUsuario = id },
                commandType: CommandType.StoredProcedure);

            return filas > 0;
        }
    }
}