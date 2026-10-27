using Domain.DTOs;

namespace WebApplication2.Services
{
    public interface IRolService
    {
        Task<IEnumerable<RolResponseDto>> GetAllAsync();
        Task<RolResponseDto?> GetByIdAsync(int id);
        Task<RolResponseDto> CreateAsync(RolCreateDto dto);
        Task<bool> UpdateAsync(int id, RolUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}