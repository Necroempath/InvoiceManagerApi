using AutoMapper;
using InvoiceManagerApi.DTOs.CustomerDTOs;
using InvoiceManagerApi.DTOs.InvoiceDTOs;
using InvoiceManagerApi.Enums;
using InvoiceManagerApi.Models;

namespace InvoiceManagerApi.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Customer

        CreateMap<Customer, CustomerResponseDto>()
            .ForMember(dest => dest.InvoiceCount,
                opt => opt.MapFrom(src => src.Invoices.Count()))
            .ForMember(dest => dest.InvoicesSum,
                opt => opt.MapFrom(src =>
                    src.Invoices.Sum(i => i.TotalSum)));

        CreateMap<CustomerCreateRequest, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());

        CreateMap<CustomerUpdateRequest, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());

        // Invoice

        CreateMap<Invoice, InvoiceResponseDto>()
            .ForMember(dest => dest.Status,
            opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.TotalSum,
            opt => opt.MapFrom(src => src.Rows.Sum(ir => ir.Sum)))
            .ForMember(dest => dest.RowsCount,
            opt => opt.MapFrom(src => src.Rows.Count()))
            .ForMember(dest => dest.CustomerName,
            opt => opt.MapFrom(src => src.Customer.Name))
            .ForMember(dest => dest.CustomerEmail,
            opt => opt.MapFrom(src => src.Customer.Email));

        CreateMap<InvoiceCreateRequest, Invoice>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Customer, opt => opt.Ignore());

        CreateMap<InvoiceUpdateRequest, Invoice>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Customer, opt => opt.Ignore());
    }
}
