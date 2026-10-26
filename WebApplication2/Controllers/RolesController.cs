using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRolService _service;

        public RolesController(IRolService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolResponseDto>>> GetRoles()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RolResponseDto>> GetRol(int id)
        {
            var rol = await _service.GetByIdAsync(id);

            if (rol == null)
                return NotFound();

            return Ok(rol);
        }

        [HttpPost]
        public async Task<ActionResult<RolResponseDto>> PostRol(RolCreateDto dto)
        {
            var creado = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(GetRol), new { id = creado.PKRol }, creado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRol(int id, RolUpdateDto dto)
        {
            var actualizado = await _service.UpdateAsync(id, dto);

            if (!actualizado)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            if (await _service.TieneUsuariosAsync(id))
                return Conflict("No se puede eliminar: hay usuarios con este rol.");

            var eliminado = await _service.DeleteAsync(id);

            if (!eliminado)
                return NotFound();

            return NoContent();
        }
    }
}