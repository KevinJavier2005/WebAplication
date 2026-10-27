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
        private readonly ILogger<RolesController> _logger;

        public RolesController(IRolService service, ILogger<RolesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolResponseDto>>> GetRoles()
        {
            try
            {
                return Ok(await _service.GetAllAsync());
            }
            catch (Exception ex)
            {
                return ErrorInterno(ex, "listar los roles");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RolResponseDto>> GetRol(int id)
        {
            try
            {
                var rol = await _service.GetByIdAsync(id);

                if (rol == null)
                    return NotFound(new { mensaje = $"No existe el rol con id {id}." });

                return Ok(rol);
            }
            catch (Exception ex)
            {
                return ErrorInterno(ex, "obtener el rol");
            }
        }

        [HttpPost]
        public async Task<ActionResult<RolResponseDto>> PostRol(RolCreateDto dto)
        {
            try
            {
                var creado = await _service.CreateAsync(dto);

                return CreatedAtAction(nameof(GetRol), new { id = creado.PKRol }, creado);
            }
            catch (Exception ex)
            {
                return ErrorInterno(ex, "crear el rol");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRol(int id, RolUpdateDto dto)
        {
            try
            {
                var actualizado = await _service.UpdateAsync(id, dto);

                if (!actualizado)
                    return NotFound(new { mensaje = $"No existe el rol con id {id}." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return ErrorInterno(ex, "actualizar el rol");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            try
            {
                var eliminado = await _service.DeleteAsync(id);

                if (!eliminado)
                    return NotFound(new { mensaje = $"No existe el rol con id {id}." });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return ErrorInterno(ex, "eliminar el rol");
            }
        }

        private ObjectResult ErrorInterno(Exception ex, string accion)
        {
            _logger.LogError(ex, "Error al {Accion}", accion);

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { mensaje = $"Ocurrió un error al {accion}. Intenta de nuevo más tarde." });
        }
    }
}