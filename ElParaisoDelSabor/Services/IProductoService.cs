using ElParaisoDelSabor.Data.Models;

namespace ElParaisoDelSabor.Services;

public interface IProductoService
{
    Task<List<Producto>> GetProductosAsync();
    Task<List<Producto>> GetProductosInactivosAsync(); // <-- Nuevo
    Task<Producto?> GetProductoByIdAsync(int id);
    Task<bool> GuardarProductoAsync(Producto producto);
    Task<bool> EliminarLogicoAsync(int id);
    Task<bool> ReactivarProductoAsync(int id); // <-- Nuevo
}