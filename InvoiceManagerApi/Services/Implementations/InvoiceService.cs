using AutoMapper;
using InvoiceManagerApi.Data;
using InvoiceManagerApi.DTOs.InvoiceDTOs;
using InvoiceManagerApi.Enums;
using InvoiceManagerApi.Models;
using InvoiceManagerApi.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManagerApi.Services.Implementations;

public class InvoiceService : IInvoiceService
{
    private readonly InvoiceManagerDbContext _context;
    private readonly IMapper _mapper;

    public InvoiceService(InvoiceManagerDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InvoiceResponseDto?> CreateAsync(InvoiceCreateRequest request)
    {
        var isCustomerExists = await _context.Customers.AnyAsync(c => c.DeletedAt == null && c.Id == request.CustomerId);
    
        if (!isCustomerExists) return null;

        var invoice = _mapper.Map<Invoice>(request);

        await _context.Invoices.AddAsync(invoice);

        await _context.SaveChangesAsync();

        return _mapper.Map<InvoiceResponseDto>(invoice);
    }

    public async Task<bool> DeleteHardAsync(int id)
    {
        Invoice? invoice = await _context.Invoices
            .FirstOrDefaultAsync(i => i.DeletedAt == null && i.Id == id);

        if (invoice is null) return false;

        _context.Invoices.Remove(invoice);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteSoftAsync(int id)
    {
        Invoice? invoice = await _context.Invoices
            .FirstOrDefaultAsync(i => i.DeletedAt == null && i.Id == id);

        if (invoice is null) return false;

        invoice.DeletedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<InvoiceResponseDto>> GetAllAsync()
    {
        var invoices = await _context.Invoices
                        .Where(i => i.DeletedAt == null)
                        .Include(i => i.Customer)
                        .Include(i => i.Rows)
                        .ToListAsync();

        return _mapper.Map<IEnumerable<InvoiceResponseDto>>(invoices);
    }

    public async Task<InvoiceResponseDto?> GetByIdAsync(int id)
    {
        var invoice = await _context.Invoices
                        .Include(i => i.Customer)
                        .Include(i => i.Rows)
                        .FirstOrDefaultAsync(i => i.DeletedAt == null && i.Id == id);

        if (invoice is null) return null;

        return _mapper.Map<InvoiceResponseDto>(invoice);
    }

    public async Task<InvoiceResponseDto?> StatusChangeAsync(int id, InvoiceStatusChangeRequest request)
    {
        var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Rows)
                .FirstOrDefaultAsync(i => i.DeletedAt == null && i.Id == id);

        if (invoice is null) return null;
        
        invoice.Status = request.Status;

        await _context.SaveChangesAsync();

        return _mapper.Map<InvoiceResponseDto>(invoice);
    }

    public async Task<InvoiceResponseDto?> UpdateAsync(int id, InvoiceUpdateRequest request)
    {
        var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Rows)
                .FirstOrDefaultAsync(i => i.DeletedAt == null && i.Id == id);

        if (invoice is null) return null;

        _mapper.Map(request, invoice);

        await _context.SaveChangesAsync();

        return _mapper.Map<InvoiceResponseDto>(invoice);
    }
}
