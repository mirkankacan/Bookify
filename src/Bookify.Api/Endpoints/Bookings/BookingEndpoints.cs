using Bookify.Application.Bookings.GetBooking;
using Bookify.Application.Bookings.ReserveBooking;
using Carter;
using MediatR;

namespace Bookify.Api.Endpoints.Bookings
{
    public class BookingEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/bookings")
              .WithTags("Bookings");

            group.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new GetBookingByIdQuery(id);
                var result = await sender.Send(query, cancellationToken);
                return Results.Ok(result);
            });

            group.MapPost("/", async (ReserveBookingCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(command, cancellationToken);
                return result.IsSuccess
                    ? Results.CreatedAtRoute("GetBooking", new { id = result.Value }, result.Value)
                    : Results.BadRequest(result.Error);
            });
        }
    }
}