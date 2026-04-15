using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Bookings.Dtos;

namespace Bookify.Application.Bookings.GetBooking
{
    public sealed record GetBookingByIdQuery(Guid BookingId) : IQuery<BookingDto>;
}