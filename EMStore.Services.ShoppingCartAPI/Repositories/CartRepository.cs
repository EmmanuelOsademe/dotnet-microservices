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
            
            
            var cartHeaderFromDB = await _dbContext.CartHeaders.FirstOrDefaultAsync(c => c.UserId == cartDto.CartHeader.UserId);
            if(cartHeaderFromDB == null)
            {
                // Create cart header and details for the user
                CartHeader cartHeader = cartDto.CartHeader.ToCartHeaderFromCartHeaderDto();
                _dbContext.CartHeaders.Add(cartHeader);
                await _dbContext.SaveChangesAsync();

                // Create CartDetails
                cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                CartDetails cartDetails = cartDto.CartDetails.First().ToCartDetailsFromCartDetailsDto();
                _dbContext.CartDetails.Add(cartDetails);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                // Check if the cart details for the existing user has the same product
                var cartDetailsFromDb = await _dbContext.CartDetails.FirstOrDefaultAsync(
                    c => c.ProductId == cartDto.CartDetails.First().ProductId && c.CartHeaderId == cartHeaderFromDB.CartHeaderId);

                if(cartDetailsFromDb == null)
                {
                    // Create a new entry for cart details
                    cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDB.CartHeaderId;
                    CartDetails cartDetails = cartDto.CartDetails.First().ToCartDetailsFromCartDetailsDto();
                    _dbContext.CartDetails.Add(cartDetails);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    // Update count in the details
                    cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
                    cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                    cartDto.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartHeaderId;
                    CartDetails cartDetails = cartDto.CartDetails.First().ToCartDetailsFromCartDetailsDto();
                    _dbContext.CartDetails.Update(cartDetails);
                    await _dbContext.SaveChangesAsync();
                }
            }

            return cartDto;

        }
    }
}
