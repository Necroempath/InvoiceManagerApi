using InvoiceManagerApi.Enums;

namespace InvoiceManagerApi.DTOs.InvoiceDTOs;

public class InvoiceUpdateRequest
{
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public string? Comment { get; set; }
}
