using InvoiceManagerApi.Enums;

namespace InvoiceManagerApi.DTOs.InvoiceRowDTOs;

public class InvoiceRowResponseDto
{
    public int Id { get; set; }
    public string Service { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal Rate { get; set; }
    public decimal Sum { get; set; }
    public int InvoiceId { get; set; }
    public string InvoiceStatus { get; set; } = string.Empty;
}
