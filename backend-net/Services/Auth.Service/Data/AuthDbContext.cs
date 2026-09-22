using Auth.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Service.Data;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>()
            .HasIndex(usuario => usuario.NombreUsuario)
            .IsUnique();
            modelBuilder.Entity<Usuario>()
    .HasIndex(usuario => usuario.NombreUsuario)
    .IsUnique();

modelBuilder.Entity<Usuario>()
    .HasIndex(usuario => usuario.Correo)
    .IsUnique();
    }
}