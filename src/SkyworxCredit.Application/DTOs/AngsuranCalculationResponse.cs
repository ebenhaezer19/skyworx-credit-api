namespace SkyworxCredit.Application.DTOs;

public class AngsuranCalculationResponse
{
    public decimal Plafon { get; set; }
    public decimal Bunga { get; set; }
    public int Tenor { get; set; }
    public decimal AngsuranPerBulan { get; set; }
    public decimal TotalPembayaran { get; set; }
    public decimal TotalBunga { get; set; }
}