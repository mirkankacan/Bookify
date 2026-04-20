using Bookify.Application.Users.RegisterUser;
using Carter;
using MediatR;

namespace Bookify.Api.Endpoints.Users
{
    public class UserEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/users")
              .WithTags("Users");

            group.MapPost("/register", async (RegisterUserCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(command, cancellationToken);
                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
            }).AllowAnonymous();
        }
    }
}