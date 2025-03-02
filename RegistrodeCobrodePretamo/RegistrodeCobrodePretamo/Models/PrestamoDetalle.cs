namespace RegistrodeCobrodePretamo.Models;

public class PrestamosDetalle
{
    public int Id { get; set; }
    public int PrestamoId { get; set; }
    public int CuotaNo { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Valor { get; set; }
    public decimal Balance { get; set; }
}
