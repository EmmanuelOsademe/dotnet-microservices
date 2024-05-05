using EMStore.Services.EmailAPI.Data;
using EMStore.Services.EmailAPI.Dtos.Cart;
using EMStore.Services.EmailAPI.Models;
using EMStore.Services.EmailAPI.Repositories.Interfaces;
using EMStore.Services.EmailAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace EMStore.Services.EmailAPI.Services
{
    public class EmailService (DbContextOptions<ApplicationDbContext> dbOptions) : IEmailService
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbOptions = dbOptions;

        public async Task EmailCartAndLog(CartDto cartDto)
        {
            StringBuilder message = new();

            message.AppendLine("<br/>Cart Email Requested");
            message.AppendLine($"<br/>Total {cartDto.CartHeader.CartTotal}");
            message.Append("<br/>");
            message.Append("<ul>");
            foreach(var item in cartDto.CartDetails)
            {
                message.Append("<li>");
                message.Append(item.Product.Name + " x " + item.Count);
                message.Append("</li>");
            }
            message.Append("</ul>");

            await LogAndEmailAsync(message.ToString(), cartDto.CartHeader.Email);

        }

        private async Task<bool> LogAndEmailAsync(string message, string email)
        {
            try
            {
                EmailLogger emailLog = new()
                {
                    Email = email,
                    EmailSent = DateTime.Now,
                    Message = message
                };

                await using var _applicationDb = new ApplicationDbContext(_dbOptions);
                await _applicationDb.EmailLoggers.AddAsync(emailLog);
                await _applicationDb.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
