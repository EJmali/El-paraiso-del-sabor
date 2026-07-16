using System.Net.Http.Json;
using Microsoft.Extensions.Hosting;

namespace ElParaisoDelSabor.Services;

public class DollarBackgroundService : BackgroundService
{
    private readonly HttpClient _httpClient;
    private readonly DollarService _dollarService;
    private int _ultimaHoraActualizada = -1;

    public DollarBackgroundService(HttpClient httpClient, DollarService dollarService)
    {
        _httpClient = httpClient;
        _dollarService = dollarService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
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
            var resultado = await _httpClient.GetFromJsonAsync<DollarRate>("v1/dolares/oficial");
            if (resultado != null)
            {
                _dollarService.PrecioActual = resultado.Promedio;
            }
        }
        catch
        {
        }
    }
}