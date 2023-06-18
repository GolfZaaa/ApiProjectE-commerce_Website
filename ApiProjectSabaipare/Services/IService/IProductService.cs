using ApiProjectSabaipare.DTOs.ProductDto;
using ApiProjectSabaipare.Models;

namespace ApiProjectSabaipare.Services.IService
{
    public interface IProductService
    {
        Task<List<Product>> GetProductListAsync();
        Task CreateAsync(ProductRequest request);
        Task<List<string>> GetTypeAsync();
    }
}
