using ElParaisoDelSabor.Data.Models;

namespace ElParaisoDelSabor.Services;

public class CarritoService
{
    private readonly List<ItemCarrito> _items = new();

    // Exponer la lista como de solo lectura para proteger los datos externos
    public IReadOnlyList<ItemCarrito> Items => _items.AsReadOnly();

    // Evento para notificar a los componentes que algo cambió
    public event Action? OnChange;

    public void AgregarProducto(Producto producto, int cantidad = 1)
    {
        var itemExistente = _items.FirstOrDefault(i => i.Producto.Id == producto.Id);

        if (itemExistente != null)
        {
            itemExistente.Cantidad += cantidad;
        }
        else
        {
            _items.Add(new ItemCarrito { Producto = producto, Cantidad = cantidad });
        }

        NotificarCambio();
    }

    public void EliminarProducto(int productoId)
    {
        var item = _items.FirstOrDefault(i => i.Producto.Id == productoId);
        if (item != null)
        {
            _items.Remove(item);
            NotificarCambio();
        }
    }

    public decimal ObtenerPrecioTotal() => _items.Sum(i => i.Total);
    
    public int ObtenerCantidadTotal()
    {
      int total = _items.Sum(i => i.Cantidad);
      Console.WriteLine($"Total registros: {total}");
      return total;
    }

    public void LimpiarCarrito()
    {
        _items.Clear();
        NotificarCambio();
    }

    private void NotificarCambio() => OnChange?.Invoke();
}