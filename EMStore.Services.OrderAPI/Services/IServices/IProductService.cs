using EMStore.Services.OrderAPI.Dtos;

namespace EMStore.Services.OrderAPI.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
