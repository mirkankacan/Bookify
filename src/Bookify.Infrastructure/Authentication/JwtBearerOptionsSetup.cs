using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Bookify.Infrastructure.Authentication
{
    internal sealed class JwtBearerOptionsSetup(AuthenticationOptions authenticationOptions) : IConfigureNamedOptions<JwtBearerOptions>
    {
        public void Configure(string? name, JwtBearerOptions options) => Configure(options);

        public void Configure(JwtBearerOptions options)
        {
            options.Audience = authenticationOptions.Audience;
            options.MetadataAddress = authenticationOptions.MetadataUrl;
            options.RequireHttpsMetadata = authenticationOptions.RequireHttpsMetadata;
            options.TokenValidationParameters.ValidIssuer = authenticationOptions.Issuer;
            options.TokenValidationParameters.ValidAudience = authenticationOptions.Audience;
        }
    }
}