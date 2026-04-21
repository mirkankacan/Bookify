using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Users.Dtos;

namespace Bookify.Application.Users.LoginUser
{
    public sealed record LoginUserCommand(string Email, string Password) : ICommand<AccessTokenDto>;
}