using EMStore.Services.ShoppingCartAPI.Data;
using EMStore.Services.ShoppingCartAPI.Dtos;
using EMStore.Services.ShoppingCartAPI.Mappers;
using EMStore.Services.ShoppingCartAPI.Models;
using EMStore.Services.ShoppingCartAPI.Repositories.Interfaces;
using EMStore.Services.ShoppingCartAPI.Services.IServices;
using EMStores.MessageBus;
using Microsoft.EntityFrameworkCore;

namespace EMStore.Services.ShoppingCartAPI.Repositories
{
    public class CartRepository(ApplicationDbContext dbContext, IProductService productService, ICouponService couponService, IMessageBus messageBus, IConfiguration config) : ICartRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IProductService _productService = productService;
        private readonly ICouponService _couponService = couponService;
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IConfiguration _config = config;
        public async Task<CartDto> UpsertCartAsync(CartInputDto cartInputDto)
        {
            // Convert the input DTO to CartDto
            CartDto cartDto = cartInputDto.ToCartDtoFromCartInputDto();
            
            
            var cartHeaderFromDB = await _dbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(c => c.UserId == cartDto.CartHeader.UserId);
            if(cartHeaderFromDB == null)
            {
                // Create cart header and details for the user
                CartHeader cartHeader = cartDto.CartHeader.ToCartHeaderFromCartHeaderDto();
                _dbContext.CartHeaders.Add(cartHeader);
                await _dbContext.SaveChangesAsync();

                // Create CartDetails
                CartDetailsDto cartDetailsDto = cartDto.CartDetails.First();
                cartDetailsDto.CartHeaderId = cartHeader.CartHeaderId;
                CartDetails cartDetails = cartDetailsDto.ToCartDetailsFromCartDetailsDto();
                _dbContext.CartDetails.Add(cartDetails);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                // Check if the cart details for the existing user has the same product
                var cartDetailsFromDb = await _dbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                    c => c.ProductId == cartDto.CartDetails.First().ProductId && c.CartHeaderId == cartHeaderFromDB.CartHeaderId);

                if(cartDetailsFromDb == null)
                {
                    // Create a new entry for cart details
                    CartDetailsDto cartDetailsDto = cartDto.CartDetails.First();
                    cartDetailsDto.CartHeaderId = cartHeaderFromDB.CartHeaderId;
                    CartDetails cartDetails = cartDetailsDto.ToCartDetailsFromCartDetailsDto();
                    _dbContext.CartDetails.Add(cartDetails);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    // Update count in the details
                    CartDetailsDto cartDetailsDto = cartDto.CartDetails.First();
                    cartDetailsDto.Count += cartDetailsFromDb.Count;
                    cartDetailsDto.CartHeaderId = cartDetailsFromDb.CartHeaderId;
                    cartDetailsDto.CartDetailsId = cartDetailsFromDb.CartDetailsId;
                    CartDetails cartDetails = cartDetailsDto.ToCartDetailsFromCartDetailsDto();
                    _dbContext.CartDetails.Update(cartDetails);
                    await _dbContext.SaveChangesAsync();
                }
            }

            return cartDto;

        }

        public async Task<bool> RemoveCartAsync(RemoveCartDto removeCartDto)
        {
            // Check that the user has exisiting cart
            var cartHeaderFromDB = await _dbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(c => c.UserId == removeCartDto.UserId);
            if (cartHeaderFromDB == null) return false;

            // Check that the cartDetails exists and that it belongs to the current user
            var cartDetailsFromDb = await _dbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                    c => c.CartDetailsId == removeCartDto.CartDetailsId && c.CartHeaderId == cartHeaderFromDB.CartHeaderId);
            if (cartDetailsFromDb == null) return false;

            // Count the number of items in the cart
            int totalCountOfCartItems = _dbContext.CartDetails.Where(c => c.CartHeaderId == cartHeaderFromDB.CartHeaderId).Count();

            // Remove the cart details
            _dbContext.CartDetails.Remove(cartDetailsFromDb);
            if (totalCountOfCartItems == 1)
            {
                // If the item is the last in the cart items, then remove the header as well
                var cartHeaderToRemove = await _dbContext.CartHeaders.FirstOrDefaultAsync(c => c.CartHeaderId == cartHeaderFromDB.CartHeaderId);
                _dbContext.CartHeaders.Remove(cartHeaderToRemove);
            }
            await _dbContext.SaveChangesAsync();


            return true;
        }

        public async Task<CartDto?> GetCartByUserIdAsync(string userId)
        {
            var cartHeader = await _dbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(c => c.UserId == userId);
            if (cartHeader == null) return null;
            CartHeaderDto cartHeaderDto = cartHeader.ToDtoFromCartHeader();

            var cartDetails = await _dbContext.CartDetails.Where(c => c.CartHeaderId == cartHeader.CartHeaderId).ToListAsync();

            var cartDetailsDto = cartDetails.Select(c => c.ToDtoFromCartDetails());

            var products = await _productService.GetProducts();

            List<CartDetailsDto> resDetailsDto = [];
            

            foreach(var item in cartDetailsDto)
            {
                item.CartHeader = cartHeaderDto;
                var product = products.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product == null) continue;
                item.Product = product;
                cartHeaderDto.CartTotal += (item.Count * product.Price );
                resDetailsDto.Add(item);
            }

            if (!string.IsNullOrEmpty(cartHeader.CouponCode))
            {
                CouponDto coupon = await _couponService.GetCouponAsync(cartHeader.CouponCode);
                if(coupon != null && cartHeaderDto.CartTotal >= coupon.MinAmount)
                {
                    cartHeaderDto.CartTotal -= coupon.DiscountAmount;
                    cartHeaderDto.Discount = coupon.DiscountAmount;
                }
            }

            return new CartDto
            {
                CartDetails = resDetailsDto,
                CartHeader = cartHeaderDto
            };
        }

        public async Task<bool> ApplyCouponAsync(CartInputDto dto)
        {
            var cartFromDb = await _dbContext.CartHeaders.FirstOrDefaultAsync(c => c.UserId == dto.CartHeaderInputDto.UserId);
            if (cartFromDb == null)
            {
                return false;
            }

            cartFromDb.CouponCode = dto.CartHeaderInputDto.CouponCode;

            _dbContext.CartHeaders.Update(cartFromDb);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveCouponAsync(CartInputDto dto)
        {
            var cartFromDb = await _dbContext.CartHeaders.FirstOrDefaultAsync(c => c.UserId == dto.CartHeaderInputDto.UserId);
            if (cartFromDb == null)
            {
                return false;
            }

            cartFromDb.CouponCode = "";
            _dbContext.CartHeaders.Update(cartFromDb);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task EmailCartAsync(CartDto cartDto)
        {
            string serviceBusConnectionString = _config.GetValue<string>("ServiceBusConnectionString") ?? string.Empty;
            string topicQueueName = _config.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue") ?? string.Empty;
            await _messageBus.PublishMessage(cartDto, topicQueueName, serviceBusConnectionString);
        }
    }
}
