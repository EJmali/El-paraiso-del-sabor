namespace ElParaisoDelSabor.Data.Models;

public class ItemCarrito
{
    public Producto Producto { get; set; } = null!;
    public int Cantidad { get; set; }
    public decimal Total => Producto.Precio * Cantidad;
}