using ElParaisoDelSabor.Data;
using ElParaisoDelSabor.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ElParaisoDelSabor.Services;

public class ProductoService : IProductoService
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

    // Inyección de dependencias por constructor de la factoría segura para Blazor
    public ProductoService(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    // Obtener todos los helados activos para el catálogo
    public async Task<List<Producto>> GetProductosAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Productos
            .Where(p => p.Activo) // Solo traemos los que no estén borrados lógicamente
            .ToListAsync();
    }

    // Buscar un helado específico por su ID
    public async Task<Producto?> GetProductoByIdAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Productos.FindAsync(id);
    }

    // Crear o actualizar un producto (Upsert)
    public async Task<bool> GuardarProductoAsync(Producto producto)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        if (producto.Id == 0)
        {
            
            // EVITA DUPLICADOS: Si ya existe un helado con ese nombre (ignorando mayúsculas/minúsculas), cancela
            if (await context.Productos.AnyAsync(p => p.Nombre.ToLower() == producto.Nombre.ToLower()))
            {
                return false; 
            }
            
            producto.Activo = true; // Aseguramos que nazca activo para que se vea en el catálogo
            context.Productos.Add(producto); // Es nuevo
        }
        else
        {
            context.Productos.Update(producto); // Ya existe, actualiza
        }

        return await context.SaveChangesAsync() > 0;
    }

    // Borrado lógico para proteger el historial de pedidos
    public async Task<bool> EliminarLogicoAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var producto = await context.Productos.FindAsync(id);
        
        if (producto == null) return false;

        producto.Activo = false; // Desactivado, oculto del catálogo
        return await context.SaveChangesAsync() > 0;
    }
}