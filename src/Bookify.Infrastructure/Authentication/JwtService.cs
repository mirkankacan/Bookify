using Bookify.Application.Abstractions.Authentication;
using Bookify.Infrastructure.Authentication.Models;
using System.Net.Http.Json;

namespace Bookify.Infrastructure.Authentication
{
    internal class JwtService(HttpClient httpClient, KeycloakOptions keycloakOptions) : IJwtService
    {
        public async Task<string?> GenerateAccessTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            try
            {
                var authRequestParameters = new KeyValuePair<string, string>[]
                {
                    new("client_id",keycloakOptions.AuthClientId),
                    new("client_secret",keycloakOptions.AuthClientSecret),
                    new("username",email),
                    new("password",password),
                    new("grant_type","password"),
                    new("scope","openid email"),
                };
                var authorizationRequestContent = new FormUrlEncodedContent(authRequestParameters);
                var response = await httpClient.PostAsync(keycloakOptions.TokenUrl, authorizationRequestContent, cancellationToken);
                response.EnsureSuccessStatusCode();
                var authorizationToken = await response.Content.ReadFromJsonAsync<AuthorizationToken>(cancellationToken);
                if (authorizationToken is null)
                    return null;

                return authorizationToken.AccessToken;
            }
            catch (Exception ex) { throw; }
        }
    }
}