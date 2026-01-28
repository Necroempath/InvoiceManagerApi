using AutoMapper;
using InvoiceManagerApi.DTOs.CustomerDTOs;
using InvoiceManagerApi.Models;

namespace InvoiceManagerApi.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerResponseDto>()
            .ForMember(dest => dest.InvoiceStatus,
                opt => opt.MapFrom(src =>
                    src.Invoice != null ? src.Invoice.Status.ToString() : null))
            .ForMember(dest => dest.InvoiceSum,
                opt => opt.MapFrom(src =>
                    src.Invoice != null ? src.Invoice.TotalSum : (decimal?)null));

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
    }
}
