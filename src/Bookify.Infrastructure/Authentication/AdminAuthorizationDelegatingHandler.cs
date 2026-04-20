using Bookify.Infrastructure.Authentication.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Bookify.Infrastructure.Authentication
{
    internal sealed class AdminAuthorizationDelegatingHandler(KeycloakOptions keycloakOptions) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AuthorizationToken authorizationToken = await GetAuthorizationTokenAsync(cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, authorizationToken.AccessToken);
            HttpResponseMessage httpResponseMessage = await base.SendAsync(request, cancellationToken);
            httpResponseMessage.EnsureSuccessStatusCode();
            return httpResponseMessage;
        }

        private async Task<AuthorizationToken> GetAuthorizationTokenAsync(CancellationToken cancellationToken)
        {
            var authorizationRequestParameters = new KeyValuePair<string, string>[]
            {
                new("client_id",keycloakOptions.AdminClientId),
                new("client_secret",keycloakOptions.AdminClientSecret),
                new("scope","openid email"),
                new("grant_type","client_credentials")
            };
            var authorizationRequestContent = new FormUrlEncodedContent(authorizationRequestParameters);
            using var authorizationRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(keycloakOptions.TokenUrl))
            {
                Content = authorizationRequestContent
            };
            HttpResponseMessage authorizationResponse = await base.SendAsync(authorizationRequest, cancellationToken);
            authorizationResponse.EnsureSuccessStatusCode();
            return await authorizationResponse.Content.ReadFromJsonAsync<AuthorizationToken>(cancellationToken) ?? throw new ApplicationException();
        }
    }
}