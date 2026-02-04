namespace InvoiceManagerApi.DTOs.InvoiceRowDTOs;

public class InvoiceRowCreateRequest
{
    public string Service { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal Rate { get; set; }
    public int InvoiceId { get; set; }
}
