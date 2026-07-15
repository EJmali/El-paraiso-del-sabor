using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElParaisoDelSabor.Data.Models;

public class Producto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public string Descripcion { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Precio { get; set; }

    [Required]
    public int Stock { get; set; } // Cantidad de litros o unidades disponibles

    // URL o ruta local de la foto del helado
    public string? ImagenUrl { get; set; } 
    
    public bool Activo { get; set; } = true; // Soft delete (borrado lógico)
}