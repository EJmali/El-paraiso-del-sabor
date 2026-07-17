using ElParaisoDelSabor.Data;
using ElParaisoDelSabor.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ElParaisoDelSabor.Services;

public class AuthService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public Usuario? UsuarioActual { get; private set; }
    public bool EstaAutenticado => UsuarioActual != null;

    public event Action? OnChange;

    public AuthService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public bool IniciarSesion(string username, string password)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(); // 👈 Cambia por el nombre de tu DbContext

        // Buscamos directamente en la tabla de SQLite
        var usuario = context.Usuarios.FirstOrDefault(u => 
            u.Username.ToLower() == username.ToLower() && 
            u.Password == password);

        if (usuario != null)
        {
            UsuarioActual = usuario;
            OnChange?.Invoke();
            return true;
        }
        return false;
    }

    public bool RegistrarUsuario(string username, string nombre, string password)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Evitamos duplicados en la base de datos
        if (context.Usuarios.Any(u => u.Username.ToLower() == username.ToLower()))
        {
            return false;
        }

        var nuevoUsuario = new Usuario
        {
            Username = username.ToLower(),
            Nombre = nombre,
            Password = password,
            Rol = "Cliente"
        };

        context.Usuarios.Add(nuevoUsuario);
        context.SaveChanges();
        return true;
    }

    public void CerrarSesion()
    {
        UsuarioActual = null;
        OnChange?.Invoke();
    }
}