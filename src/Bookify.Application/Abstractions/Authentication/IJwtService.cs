namespace Bookify.Application.Abstractions.Authentication
{
    public interface IJwtService
    {
        Task<string?> GenerateAccessTokenAsync(string email, string password, CancellationToken cancellationToken = default);
    }
}