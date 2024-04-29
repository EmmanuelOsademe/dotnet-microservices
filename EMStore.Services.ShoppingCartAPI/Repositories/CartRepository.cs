using EMStore.Services.ShoppingCartAPI.Data;
using EMStore.Services.ShoppingCartAPI.Dtos;
using EMStore.Services.ShoppingCartAPI.Mappers;
using EMStore.Services.ShoppingCartAPI.Models;
using EMStore.Services.ShoppingCartAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace EMStore.Services.ShoppingCartAPI.Repositories
{
    public class CartRepository(ApplicationDbContext dbContext) : ICartRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
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

            foreach(var item in cartDetailsDto)
            {
                cartHeaderDto.CartTotal += (item.Count * item.Product.Price);
            }

            return new CartDto
            {
                CartDetails = cartDetailsDto,
                CartHeader = cartHeaderDto
            };
        }
    }
}
