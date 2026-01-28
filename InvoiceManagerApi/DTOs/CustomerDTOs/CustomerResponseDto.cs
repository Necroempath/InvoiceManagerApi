namespace InvoiceManagerApi.DTOs.CustomerDTOs;

public class CustomerResponseDto()
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public decimal? InvoiceSum { get; set; }
    public string? InvoiceStatus { get; set; }
}
