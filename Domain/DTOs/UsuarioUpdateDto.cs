namespace Domain.DTOs
{
    public class UsuarioUpdateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string? Password { get; set; }   // si viene vacío, no se cambia
        public bool Estatus { get; set; }
        public int FKRol { get; set; }
    }
}