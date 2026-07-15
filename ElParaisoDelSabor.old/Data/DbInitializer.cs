using ElParaisoDelSabor.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ElParaisoDelSabor.Data;

public static class DbInitializer
{
    public static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        // Obtenemos la factoría de DbContext desde el proveedor de servicios
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
      
        // 1. Asegurar que las migraciones estén aplicadas al arrancar
        await context.Database.MigrateAsync();

        // 2. Verificar si ya existen productos para evitar duplicados (Idempotencia)
        if (await context.Productos.AnyAsync())
        {
            return; // La base de datos ya tiene datos, no hacemos nada
        }

        // 3. Crear el listado de helados iniciales
        var productosSemilla = new List<Producto>
        {
            new Producto
            {
                Nombre = "Helado de Mantecado Tradicional",
                Descripcion = "El clásico e infaltable helado de mantecado, cremoso y con un toque sutil de vainilla gourmet.",
                Precio = 3.50m,
                Stock = 50,
                ImagenUrl = "images/helados/mantecado.png",
                Activo = true
            },
            new Producto
            {
                Nombre = "Chocolate Supremo",
                Descripcion = "Para los amantes del chocolate intenso, elaborado con cacao de alta pureza y trozos de brownie.",
                Precio = 4.00m,
                Stock = 40,
                ImagenUrl = "images/helados/chocolate.png",
                Activo = true
            },
            new Producto
            {
                Nombre = "Fresa Silvestre Cream",
                Descripcion = "Helado cremoso combinado con un delicioso sirope artesanal de fresas naturales.",
                Precio = 3.75m,
                Stock = 35,
                ImagenUrl = "images/helados/fresa.png",
                Activo = true
            },
            new Producto
            {
                Nombre = "Súper Tornado de Cookies & Cream",
                Descripcion = "Base cremosa de nata con una enorme cantidad de galletas de chocolate trituradas.",
                Precio = 4.50m,
                Stock = 25,
                ImagenUrl = "images/helados/cookies.png",
                Activo = true
            }
        };

        // 4. Guardar los datos en SQLite
        await context.Productos.AddRangeAsync(productosSemilla);
        await context.SaveChangesAsync();
    }
}