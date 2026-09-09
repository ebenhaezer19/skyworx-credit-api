namespace SkyworxCredit.Application.DTOs;

public class PengajuanKreditRequest
{
    public decimal Plafon { get; set; }
    public decimal Bunga { get; set; }
    public int Tenor { get; set; }
}