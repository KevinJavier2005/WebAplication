using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Rol
    {
        [Key]
        public int PKRol { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public bool Estatus { get; set; }
    }
}