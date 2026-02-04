using AutoMapper;
using InvoiceManagerApi.Data;
using InvoiceManagerApi.DTOs.CustomerDTOs;
using InvoiceManagerApi.Models;
using InvoiceManagerApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManagerApi.Services.Implementations;

public class CustomerService : ICustomerService
{
    private readonly InvoiceManagerDbContext _context;
    private readonly IMapper _mapper;

    public CustomerService(InvoiceManagerDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CustomerResponseDto> CreateAsync(CustomerCreateRequest request)
    {
        Customer customer = _mapper.Map<Customer>(request);

        await _context.Customers.AddAsync(customer);

        await _context.SaveChangesAsync();

        return _mapper.Map<CustomerResponseDto>(customer);
    }

    public async Task<IEnumerable<CustomerResponseDto>> GetAllAsync()
    {
        IEnumerable<Customer> customers = await _context.Customers
            .Where(c => c.DeletedAt == null)
            .Include(c => c.Invoices.Where(i => i.DeletedAt == null))
                .ThenInclude(i => i.Rows)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CustomerResponseDto>>(customers);
    }

    public async Task<CustomerResponseDto?> GetByIdAsync(int id)
    {
        Customer? customer = await _context.Customers
            .Include(c => c.Invoices.Where(i => i.DeletedAt == null))
            .FirstOrDefaultAsync(c => c.DeletedAt == null && c.Id == id);

        if (customer is null) return null;

        return _mapper.Map<CustomerResponseDto>(customer);
    }

    public async Task<bool> DeleteHardAsync(int id)
    {
        Customer? customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.DeletedAt == null && c.Id == id && c.Invoices.Count() == 0);

        if (customer is null) return false;

        _context.Customers.Remove(customer);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteSoftAsync(int id)
    {
        Customer? customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.DeletedAt == null && c.Id == id);

        if (customer is null) return false;

        customer.DeletedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<CustomerResponseDto?> UpdateAsync(int id, CustomerUpdateRequest request)
    {
        Customer? customer = await _context.Customers
            .Include(c => c.Invoices)
            .FirstOrDefaultAsync(c => c.DeletedAt == null && c.Id == id);

        if (customer is null) return null;

       _mapper.Map(request, customer);

        await _context.SaveChangesAsync();

        return _mapper.Map<CustomerResponseDto>(customer);

    }
}
