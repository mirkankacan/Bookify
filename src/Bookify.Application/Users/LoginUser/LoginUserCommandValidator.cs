using FluentValidation;

namespace Bookify.Application.Users.LoginUser
{
    internal class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(5);
        }
    }
}