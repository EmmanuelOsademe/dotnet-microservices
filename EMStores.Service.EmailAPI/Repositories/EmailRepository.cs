using EMStore.Services.EmailAPI.Data;
using EMStore.Services.EmailAPI.Models;
using EMStore.Services.EmailAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EMStore.Services.EmailAPI.Repositories
{
    public class EmailRepository(DbContextOptions<ApplicationDbContext> dbOptions) : IEmailRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbOptions = dbOptions;

        public async Task<bool> LogAndEmailAsync(string message, string email)
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
