using EmStores.Services.ShoppingCartAPI.Dtos;

namespace EMStore.Services.ShoppingCartAPI.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
