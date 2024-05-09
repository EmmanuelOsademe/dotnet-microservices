using EMStore.Services.OrdersAPI.Dtos;

namespace EMStore.Services.OrdersAPI.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
