using Domain.DTOs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Context;

namespace WebApplication2.Services
{
    public class RolService : IRolService
    {
        private readonly ApplicationDBContext _context;

        public RolService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RolResponseDto>> GetAllAsync()
        {
            return await _context.Roles
                .AsNoTracking()
                .Select(r => new RolResponseDto
                {
                    PKRol = r.PKRol,
                    Nombre = r.Nombre,
                    Estatus = r.Estatus
                })
                .ToListAsync();
        }

        public async Task<RolResponseDto?> GetByIdAsync(int id)
        {
            var rol = await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.PKRol == id);

            return rol == null ? null : ToDto(rol);
        }

        public async Task<RolResponseDto> CreateAsync(RolCreateDto dto)
        {
            var rol = new Rol
            {
                Nombre = dto.Nombre,
                Estatus = dto.Estatus
            };

            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();

            return ToDto(rol);
        }

        public async Task<bool> UpdateAsync(int id, RolUpdateDto dto)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
                return false;

            rol.Nombre = dto.Nombre;
            rol.Estatus = dto.Estatus;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
                return false;

            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> TieneUsuariosAsync(int id)
        {
            return await _context.Usuarios.AnyAsync(u => u.FKRol == id);
        }

        private static RolResponseDto ToDto(Rol r) => new()
        {
            PKRol = r.PKRol,
            Nombre = r.Nombre,
            Estatus = r.Estatus
        };
    }
}