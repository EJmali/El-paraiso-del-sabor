using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElParaisoDelSabor.Data.Models;

public class DetallePedido
{
    public int Id { get; set; }

    public int PedidoId { get; set; }
    [ForeignKey(nameof(PedidoId))]
    public Pedido Pedido { get; set; } = null!;

    public int ProductoId { get; set; }
    [ForeignKey(nameof(ProductoId))]
    public Producto Producto { get; set; } = null!;

    [Required]
    public int Cantidad { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecioUnitario { get; set; } 
}