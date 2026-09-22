using Depreciacion.Domain;
using Depreciacion.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Depreciacion.Infrastructure;

public class DepreciacionDbContext : DbContext
{
    public DepreciacionDbContext(DbContextOptions<DepreciacionDbContext> options)
        : base(options)
    {
    }

    public DbSet<Activo> Activos => Set<Activo>();
    public DbSet<DetalleDepreciacion> Detalles => Set<DetalleDepreciacion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Activo>()
            .HasMany(a => a.Detalles)
            .WithOne()
            .HasForeignKey(d => d.ActivoId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Activo>()
            .Property(a => a.ValorCompra)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Activo>()
            .Property(a => a.ValorResidual)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<DetalleDepreciacion>()
            .Property(d => d.DepreciacionPeriodo)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<DetalleDepreciacion>()
            .Property(d => d.DepreciacionAcumulada)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<DetalleDepreciacion>()
            .Property(d => d.ValorLibros)
            .HasColumnType("decimal(18,2)");
    }
}