using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Authentication;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Users;

namespace Bookify.Application.Users.RegisterUser
{
    internal sealed class RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IAuthenticationService authenticationService) : ICommandHandler<RegisterUserCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = User.Create(new FirstName(request.FirstName), new LastName(request.LastName), new Email(request.Email));

            var identityId = await authenticationService.RegisterAsync(user, request.Password, cancellationToken);
            user.SetIdentityId(identityId);
            userRepository.Add(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            //var token = authenticationService.GenerateJwtToken(user, cancellationToken);
            return Result.Success<Guid>(user.Id);
        }
    }
}