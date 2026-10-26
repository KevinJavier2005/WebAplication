namespace Domain.DTOs
{
    public class UsuarioCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool Estatus { get; set; } = true;
        public int FKRol { get; set; }
    }
}