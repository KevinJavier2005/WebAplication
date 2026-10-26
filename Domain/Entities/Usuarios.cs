using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Usuarios
    {
        [Key]
        public int PKUsuario { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public bool Estatus { get; set; }

        [ForeignKey("Roles")]
        public int FKRol { get; set; }

        public Rol? Roles { get; set; }
    }
}