namespace InvoiceManagerApi.DTOs.InvoiceDTOs;

public class InvoiceCreateRequest
{
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public string? Comment { get; set; }
    public int CustomerId { get; set; }
}
