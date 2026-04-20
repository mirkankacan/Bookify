namespace Bookify.Infrastructure.Authentication
{
    internal sealed class AuthenticationOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string MetadataUrl { get; init; }
        public bool RequireHttpsMetadata { get; init; }
        public int JwtExpirationInMinutes { get; set; } = 60;
        public string JwtRefreshTokenSecret { get; set; }
        public int JwtRefreshTokenExpirationInDays { get; set; } = 30;
    }
}