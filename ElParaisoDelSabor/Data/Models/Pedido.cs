using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElParaisoDelSabor.Data.Models;

public class Pedido
{
    public int Id { get; set; }

    [Required]
    public int UsuarioId { get; set; }
    //public string UsuarioId { get; set; } = string.Empty;
    
    [ForeignKey(nameof(UsuarioId))]
    public Usuario Usuario { get; set; } = null!;

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }

    [Required]
    public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente;

    // Relación 1 a muchos: Un pedido tiene muchos detalles
    public List<DetallePedido> Detalles { get; set; } = new();
}

public enum EstadoPedido
{
    Pendiente,
    Procesando,
    Enviado,
    Entregado,
    Cancelado
}