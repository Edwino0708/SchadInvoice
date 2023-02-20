using AutoMapper;
using BusinessLogic.Entity;
using SchadInvoice.Models.Dto;

namespace SchadInvoice.Models.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<CustomerType, CustomerTypeDto>()
                .ReverseMap();

            CreateMap<Customer, CustomerDto>()
                .ReverseMap();

            CreateMap<Invoice, InvoiceDto>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.CustName))
            .ReverseMap();

            CreateMap<InvoiceDetail, InvoiceDetailDto>()
            .ReverseMap();
        }
    }
}
