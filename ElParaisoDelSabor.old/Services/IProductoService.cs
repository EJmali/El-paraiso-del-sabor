using ElParaisoDelSabor.Data.Models;

namespace ElParaisoDelSabor.Services;

public interface IProductoService
{
    Task<List<Producto>> GetProductosAsync();
    Task<Producto?> GetProductoByIdAsync(int id);
    Task<bool> GuardarProductoAsync(Producto producto);
    Task<bool> EliminarLogicoAsync(int id);
}