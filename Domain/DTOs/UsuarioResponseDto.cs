namespace Domain.DTOs
{
    public class UsuarioResponseDto
    {
        public int PKUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool Estatus { get; set; }
        public int FKRol { get; set; }
        public string? RolNombre { get; set; }
    }
}