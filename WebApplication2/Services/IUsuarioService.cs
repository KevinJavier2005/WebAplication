using Domain.DTOs;

namespace WebApplication2.Services
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioResponseDto>> GetAllAsync();
        Task<UsuarioResponseDto?> GetByIdAsync(int id);
        Task<UsuarioResponseDto> CreateAsync(UsuarioCreateDto dto);
        Task<bool> UpdateAsync(int id, UsuarioUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}