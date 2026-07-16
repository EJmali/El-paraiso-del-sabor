namespace ElParaisoDelSabor.Services;

public class DollarRate
{
    public string Moneda { get; set; } = string.Empty;
    public string Fuente { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public decimal? Compra { get; set; }
    public decimal? Venta { get; set; }
    public decimal Promedio { get; set; }
    public DateTime FechaActualizacion { get; set; }
}

public class DollarService
{
    public decimal PrecioActual { get; set; }
}