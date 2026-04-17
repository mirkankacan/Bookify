using Carter;

namespace Bookify.Api.Endpoints.Users
{
    public class UserEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/users")
              .WithTags("Users");

            // User use case'leri eklenince buraya gelecek
        }
    }
}