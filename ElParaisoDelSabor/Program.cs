using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using ElParaisoDelSabor.Components;
using ElParaisoDelSabor.Data;
using ElParaisoDelSabor.Services;
using ElParaisoDelSabor.Data.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de la Interfaz Gráfica (Blazor Server Unificado)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 2. Registro de la Base de Datos SQLite utilizando tu Contexto Personalizado
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=ElParaisoDelSabor.db";

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString),
    ServiceLifetime.Scoped);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// 3. Registro de la Capa de Lógica de Negocio (Tus Servicios)
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<CarritoService>();

// Tu autenticación personalizada: Scoped para que cada celular tenga su sesión independiente
builder.Services.AddScoped<AuthService>(); 

var app = builder.Build();

// 4. Pipeline de configuración de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

// Mapeo interactivo en el servidor
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(ElParaisoDelSabor.Client._Imports).Assembly);

// --- BLOQUE DE INICIALIZACIÓN AUTOMÁTICA DE TABLAS Y REGISTROS DE PRUEBA ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        // Crea físicamente el archivo SQLite y TODAS tus tablas (incluida tu tabla 'Usuarios')
        await context.Database.EnsureCreatedAsync(); 
        
        // Verifica si ya existe el Administrador en TU tabla personalizada para no duplicarlo
        if (!await context.Usuarios.AnyAsync(u => u.Username == "admin"))
        {
            context.Usuarios.Add(new Usuario {
                Username = "admin",
                Nombre = "Administrador",
                Password = "unefa2026",
                Rol = "Admin"
            });
            await context.SaveChangesAsync();
        }
  
        // Inicializa el catálogo de helados de prueba
        await DbInitializer.SeedDataAsync(services);
        
        Console.WriteLine("[Database] Base de datos y credenciales caseras inicializadas con éxito.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error crítico durante el ciclo de inicialización automática de datos.");
    }
}

app.Run();
