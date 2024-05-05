namespace EMStore.Services.EmailAPI.Repositories.Interfaces
{
    public interface IEmailRepository
    {
        Task<bool> LogAndEmailAsync(string message, string email);
    }
}
