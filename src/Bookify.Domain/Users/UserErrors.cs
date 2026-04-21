using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Users
{
    public static class UserErrors
    {
        public static readonly Error UserNotFound = new Error("User.NotFound", "User not found.");
        public static readonly Error AuthenticationFailed = new("Keycloak.AuthFailed", "Authentication failed. Please check your credentials.");
    }
}