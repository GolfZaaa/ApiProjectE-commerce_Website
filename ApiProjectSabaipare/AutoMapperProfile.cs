using ApiProjectSabaipare.DTOs.ProductDto;
using ApiProjectSabaipare.Models;
using AutoMapper;

namespace ApiProjectSabaipare
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<Product, ProductRequest>();
            CreateMap<ProductRequest, Product>();


            CreateMap<Product, ProductResponse>();
            CreateMap<ProductResponse, Product>();

        }
    }
}
