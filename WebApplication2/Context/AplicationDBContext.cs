using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Context
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
    }
}