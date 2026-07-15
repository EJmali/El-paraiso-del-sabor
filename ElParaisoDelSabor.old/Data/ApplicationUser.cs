using Microsoft.AspNetCore.Identity;

namespace ElParaisoDelSabor.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
  public string NombreCompleto { get; set; } = string.Empty;
  public string? DireccionEnvio { get; set; }
}

