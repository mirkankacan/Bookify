using Bookify.Application.Abstractions.Authentication;
using Bookify.Domain.Users;
using Bookify.Infrastructure.Authentication.Models;
using System.Net.Http.Json;

namespace Bookify.Infrastructure.Authentication
{
    internal class AuthenticationService(HttpClient httpClient) : IAuthenticationService
    {
        private const string PasswordCredentialType = "password";

        public async Task<string> RegisterAsync(User user, string password, CancellationToken cancellationToken = default)
        {
            var userRepresantationModel = UserRepresantationModel.FromUser(user);
            userRepresantationModel.Credentials = new CredentialRepresentationModel[]
            {
                new()
                {
                    Value=password,
                    Temprorary=false,
                    Type = PasswordCredentialType
                }
            };
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("users/register", userRepresantationModel, cancellationToken);
            return ExtractIdentityIdFromLocationHeader(response);
        }

        private static string ExtractIdentityIdFromLocationHeader(HttpResponseMessage response)
        {
            const string usersSegmentName = "users/";
            string? locationHeader = response.Headers.Location?.PathAndQuery;
            if (locationHeader is null)
                throw new InvalidOperationException("Location header can't be null");

            int userSegmentValueIndex = locationHeader.IndexOf(usersSegmentName, StringComparison.InvariantCultureIgnoreCase);
            string userIdentityId = locationHeader.Substring(userSegmentValueIndex + usersSegmentName.Length);
            return userIdentityId;
        }
    }
}