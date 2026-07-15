using System.ComponentModel.DataAnnotations;

namespace ElParaisoDelSabor.Data.Models;

public class Usuario
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    public string Nombre { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
    
    public string Rol { get; set; } = "Cliente";
}