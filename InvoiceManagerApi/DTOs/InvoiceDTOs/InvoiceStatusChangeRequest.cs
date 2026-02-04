using InvoiceManagerApi.Enums;

namespace InvoiceManagerApi.DTOs.InvoiceDTOs;

public class InvoiceStatusChangeRequest
{
    public InvoiceStatus Status { get; set; }
}
