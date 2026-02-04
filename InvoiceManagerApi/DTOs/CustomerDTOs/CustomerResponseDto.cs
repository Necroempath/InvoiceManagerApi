namespace InvoiceManagerApi.DTOs.CustomerDTOs;

public class CustomerResponseDto()
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public int InvoiceCount { get; set; }
    public decimal? InvoicesSum { get; set; }
}
