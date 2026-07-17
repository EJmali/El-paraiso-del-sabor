using Microsoft.EntityFrameworkCore;
using ElParaisoDelSabor.Data.Models;
using ElParaisoDelSabor.Data;

namespace ElParaisoDelSabor.Services;

public interface IPedidoService
{
    Task<List<Pedido>> GetPedidosAsync();
    Task<bool> ActualizarEstadoPedidoAsync(int pedidoId, EstadoPedido nuevoEstado);
    Task<bool> CrearPedidoAsync(Pedido pedido);
}

public class PedidoService : IPedidoService
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

    public PedidoService(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
  
    public async Task<bool> CrearPedidoAsync(Pedido pedido)
{
    using var context = await _contextFactory.CreateDbContextAsync();
    context.Pedidos.Add(pedido);
    return await context.SaveChangesAsync() > 0;
}
  
    public async Task<List<Pedido>> GetPedidosAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Pedidos
            .Include(p => p.Usuario) // Trae los datos de ApplicationUser
            .Include(p => p.Detalles)
                .ThenInclude(d => d.Producto) // Cruza el detalle con la tabla Producto
            .OrderByDescending(p => p.FechaCreacion)
            .ToListAsync();
    }

    public async Task<bool> ActualizarEstadoPedidoAsync(int pedidoId, EstadoPedido nuevoEstado)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var pedido = await context.Pedidos.FindAsync(pedidoId);
        if (pedido == null) return false;

        pedido.Estado = nuevoEstado;
        return await context.SaveChangesAsync() > 0;
    }
}