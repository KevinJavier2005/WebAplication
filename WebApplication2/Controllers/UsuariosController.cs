using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _service;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(IUsuarioService service, ILogger<UsuariosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDto>>> GetUsuarios()
        {
            try
            {
                return Ok(await _service.GetAllAsync());
            }
            catch (Exception ex)
            {
                return ErrorInterno(ex, "listar los usuarios");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDetalleDto>> GetUsuario(int id)
        {
            try
            {
                var usuario = await _service.GetByIdAsync(id);

                if (usuario == null)
                    return NotFound(new { mensaje = $"No existe el usuario con id {id}." });

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return ErrorInterno(ex, "obtener el usuario");
            }
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioResponseDto>> PostUsuario(UsuarioCreateDto dto)
        {
            try
            {
                var creado = await _service.CreateAsync(dto);

                return CreatedAtAction(nameof(GetUsuario), new { id = creado.PKUsuario }, creado);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return ErrorInterno(ex, "crear el usuario");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioUpdateDto dto)
        {
            try
            {
                var actualizado = await _service.UpdateAsync(id, dto);

                if (!actualizado)
                    return NotFound(new { mensaje = $"No existe el usuario con id {id}." });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return ErrorInterno(ex, "actualizar el usuario");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                var eliminado = await _service.DeleteAsync(id);

                if (!eliminado)
                    return NotFound(new { mensaje = $"No existe el usuario con id {id}." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return ErrorInterno(ex, "eliminar el usuario");
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