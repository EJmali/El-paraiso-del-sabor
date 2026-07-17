using System.Net.Http.Json;
using Microsoft.Extensions.Hosting;

namespace ElParaisoDelSabor.Services;

public class DollarBackgroundService : BackgroundService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly DollarService _dollarService;
    private int _ultimaHoraActualizada = -1;

    public DollarBackgroundService(IHttpClientFactory httpClientFactory, DollarService dollarService)
    {
        _httpClientFactory = httpClientFactory;
        _dollarService = dollarService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Primera carga al encender el servidor
        await ActualizarPrecioAsync();

        using PeriodicTimer timer = new(TimeSpan.FromMinutes(5));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            var ahora = DateTime.Now;

            if ((ahora.Hour == 6 || ahora.Hour == 13 || ahora.Hour == 20) && ahora.Hour != _ultimaHoraActualizada)
            {
                await ActualizarPrecioAsync();
                _ultimaHoraActualizada = ahora.Hour;
            }
        }
    }

    private async Task ActualizarPrecioAsync()
    {
        try
        {
            // Creamos el cliente usando el nombre que configuramos en Program.cs
            var client = _httpClientFactory.CreateClient("DolarApi");
            var resultado = await client.GetFromJsonAsync<DollarRate>("https://ve.dolarapi.com/v1/dolares/oficial");
            
            if (resultado != null)
            {
                _dollarService.PrecioActual = resultado.Promedio;
                Console.WriteLine($"[DollarAPI] Tasa actualizada con éxito: {resultado.Promedio} Bs.");
            }
        }
        catch (Exception ex)
        {
            // Si la API cae o no hay internet, lo mostrar directo en la consola del sistema
            Console.WriteLine($"[Error DollarAPI]: {ex.Message}");
        }
    }
}