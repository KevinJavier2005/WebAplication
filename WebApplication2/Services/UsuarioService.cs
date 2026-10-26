using Domain.DTOs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Context;

namespace WebApplication2.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDBContext _context;

        public UsuarioService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UsuarioResponseDto>> GetAllAsync()
        {
            return await _context.Usuarios
                .AsNoTracking()
                .Select(u => new UsuarioResponseDto
                {
                    PKUsuario = u.PKUsuario,
                    Nombre = u.Nombre,
                    Username = u.Username,
                    Estatus = u.Estatus,
                    FKRol = u.FKRol,
                    RolNombre = u.Roles != null ? u.Roles.Nombre : null
                })
                .ToListAsync();
        }

        public async Task<UsuarioResponseDto?> GetByIdAsync(int id)
        {
            var usuario = await _context.Usuarios
                .AsNoTracking()
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.PKUsuario == id);

            return usuario == null ? null : ToDto(usuario);
        }

        public async Task<UsuarioResponseDto> CreateAsync(UsuarioCreateDto dto)
        {
            await ValidarRolAsync(dto.FKRol);

            var usuario = new Usuarios
            {
                Nombre = dto.Nombre,
                Username = dto.Username,
                Password = dto.Password,
                Estatus = dto.Estatus,
                FKRol = dto.FKRol
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            await _context.Entry(usuario).Reference(u => u.Roles).LoadAsync();

            return ToDto(usuario);
        }

        public async Task<bool> UpdateAsync(int id, UsuarioUpdateDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return false;

            await ValidarRolAsync(dto.FKRol);

            usuario.Nombre = dto.Nombre;
            usuario.Username = dto.Username;
            usuario.Estatus = dto.Estatus;
            usuario.FKRol = dto.FKRol;

            if (!string.IsNullOrWhiteSpace(dto.Password))
                usuario.Password = dto.Password;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task ValidarRolAsync(int fkRol)
        {
            var existe = await _context.Roles.AnyAsync(r => r.PKRol == fkRol);
            if (!existe)
                throw new InvalidOperationException($"El rol con id {fkRol} no existe. Crea el rol primero.");
        }

        private static UsuarioResponseDto ToDto(Usuarios u) => new()
        {
            PKUsuario = u.PKUsuario,
            Nombre = u.Nombre,
            Username = u.Username,
            Estatus = u.Estatus,
            FKRol = u.FKRol,
            RolNombre = u.Roles?.Nombre
        };
    }
}