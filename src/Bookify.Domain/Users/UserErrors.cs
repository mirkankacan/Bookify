using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Users
{
    public static class UserErrors
    {
        public static readonly Error UserNotFound = new Error("User.NotFound", "User not found.");
    }
}