using Bookify.Application.Users.GetUserById;
using Bookify.Application.Users.LoginUser;
using Bookify.Application.Users.RegisterUser;
using Bookify.Domain.Users;
using Bookify.Infrastructure.Authentication;
using Carter;
using MediatR;
using System.Security.Claims;

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
            group.MapPost("/login", async (LoginUserCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(command, cancellationToken);
                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
            }).AllowAnonymous();

            group.MapGet("/me", async (ClaimsPrincipal principal, ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new GetUserByIdQuery(principal.GetIdentityId());
                var result = await sender.Send(query, cancellationToken);
                return result is not null ? Results.Ok(result) : Results.NotFound();
            }).RequireAuthorization(Permissions.UsersRead);
        }
    }
}