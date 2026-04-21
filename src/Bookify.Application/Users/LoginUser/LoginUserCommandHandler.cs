using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Users.Dtos;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Users;

namespace Bookify.Application.Users.LoginUser
{
    internal sealed class LoginUserCommandHandler(IJwtService jwtService, IUserRepository userRepository) : ICommandHandler<LoginUserCommand, AccessTokenDto>
    {
        public async Task<Result<AccessTokenDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var result = await jwtService.GenerateAccessTokenAsync(request.Email, request.Password, cancellationToken);
            if (string.IsNullOrEmpty(result))
                return Result.Failure<AccessTokenDto>(UserErrors.AuthenticationFailed);

            return Result.Success(new AccessTokenDto(result));
        }
    }
}