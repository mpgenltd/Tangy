using AutoMapper;
using Tangy_Models;

namespace Tangy_DataAccess;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CategoryDto, Category>().ReverseMap();
        CreateMap<ProductDto, Product>().ReverseMap();
        CreateMap<ProductPriceDto, ProductPrice>().ReverseMap();
    }
}