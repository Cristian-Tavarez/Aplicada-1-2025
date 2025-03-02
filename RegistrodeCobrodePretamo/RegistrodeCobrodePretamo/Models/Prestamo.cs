namespace RegistrodeCobrodePretamo.Models;

public class Prestamo
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public decimal Monto { get; set; }
    public int CantidadCuotas { get; set; }
    public decimal Interes { get; set; }
    public decimal MontoCuota { get; set; }
    public decimal MontoPagado { get; set; }
    public bool EstaPagado { get; set; }
    public List<PrestamosDetalle> Cuotas { get; set; } = new();
}
