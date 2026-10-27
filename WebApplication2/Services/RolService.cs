using Dapper;
using Domain.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApplication2.Context;

namespace WebApplication2.Services
{
    public class RolService : IRolService
    {
        private readonly DapperContext _context;

        public RolService(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RolResponseDto>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();

            return await connection.QueryAsync<RolResponseDto>(
                "sp_Roles_Listar",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<RolResponseDto?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<RolResponseDto>(
                "sp_Roles_ObtenerPorId",
                new { PKRol = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<RolResponseDto> CreateAsync(RolCreateDto dto)
        {
            using var connection = _context.CreateConnection();

            return await connection.QuerySingleAsync<RolResponseDto>(
                "sp_Roles_Crear",
                new { dto.Nombre, dto.Estatus },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> UpdateAsync(int id, RolUpdateDto dto)
        {
            using var connection = _context.CreateConnection();

            var filas = await connection.ExecuteAsync(
                "sp_Roles_Actualizar",
                new { PKRol = id, dto.Nombre, dto.Estatus },
                commandType: CommandType.StoredProcedure);

            return filas > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using var connection = _context.CreateConnection();

                var filas = await connection.ExecuteAsync(
                    "sp_Roles_Eliminar",
                    new { PKRol = id },
                    commandType: CommandType.StoredProcedure);

                return filas > 0;
            }
            catch (SqlException ex) when (ex.Number == 50002)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}