using InvoiceManagerApi.Enums;

namespace InvoiceManagerApi.DTOs.InvoiceRowDTOs;

public class InvoiceRowUpdateRequest
{
    public string Service { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal Rate { get; set; }
}
