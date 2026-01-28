namespace InvoiceManagerApi.DTOs.CustomerDTOs;

public class CustomerCreateRequest
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
}