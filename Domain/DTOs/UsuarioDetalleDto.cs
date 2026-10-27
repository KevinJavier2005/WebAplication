namespace Domain.DTOs
{
    public class UsuarioDetalleDto : UsuarioResponseDto
    {
        public string Password { get; set; } = string.Empty;
    }
}