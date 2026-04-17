using Bookify.Application.Apartments.SearchApartment;
using Carter;
using MediatR;

namespace Bookify.Api.Endpoints.Apartments
{
    public class ApartmentEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/apartments")
              .WithTags("Apartments");

            group.MapGet("/", async (DateOnly startDate, DateOnly endDate, ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new SearchApartmentQuery(startDate, endDate);
                var result = await sender.Send(query, cancellationToken);
                return Results.Ok(result);
            });
        }
    }
}