using ElParaisoDelSabor.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ElParaisoDelSabor.Data;

// Heredamos de IdentityDbContext pasando nuestro ApplicationUser personalizado
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Tablas de nuestro dominio de negocio
    public DbSet<Producto> Productos { get; set; } = null!;
    public DbSet<Pedido> Pedidos { get; set; } = null!;
    public DbSet<DetallePedido> DetallesPedido { get; set; } = null!;
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // CRUCIAL: Llama al base.OnModelCreating primero para que Identity configure sus propias tablas
        base.OnModelCreating(builder);

        // Configuración mediante Fluent API (Buenas prácticas de diseño)
        
        // 1. Configurar precisión decimal para precios y totales (Evita advertencias en EF Core)
        builder.Entity<Producto>()
            .Property(p => p.Precio)
            .HasPrecision(18, 2);

        builder.Entity<Pedido>()
            .Property(p => p.Total)
            .HasPrecision(18, 2);

        builder.Entity<DetallePedido>()
            .Property(dp => dp.PrecioUnitario)
            .HasPrecision(18, 2);

        // 2. Configurar la relación Muchos a Muchos implícita (Pedido -> Detalles -> Producto)
        // Si se elimina un pedido, se eliminan automáticamente sus detalles (Cascade Delete)
        builder.Entity<Pedido>()
            .HasMany(p => p.Detalles)
            .WithOne(d => d.Pedido)
            .HasForeignKey(d => d.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);

        // Si se intenta eliminar un producto (helado) que ya está en un pedido viejo, 
        // el sistema debe impedirlo para no romper el historial de ventas.
        builder.Entity<DetallePedido>()
            .HasOne(d => d.Producto)
            .WithMany()
            .HasForeignKey(d => d.ProductoId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}