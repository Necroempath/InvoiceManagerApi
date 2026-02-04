using InvoiceManagerApi.Enums;

namespace InvoiceManagerApi.DTOs.InvoiceDTOs;

public class InvoiceResponseDto
{
    public int Id { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Comment { get; set; }
    public decimal TotalSum { get; set; }
    public int RowsCount { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
}
