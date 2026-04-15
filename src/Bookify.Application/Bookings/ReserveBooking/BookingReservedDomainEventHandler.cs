using Bookify.Application.Abstractions.Email;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Bookify.Domain.Bookings.Events;
using Bookify.Domain.Users;
using MediatR;

namespace Bookify.Application.Bookings.ReserveBooking
{
    public sealed class BookingReservedDomainEventHandler(IBookingRepository bookingRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, IEmailService emailService) : INotificationHandler<BookingReservedDomainEvent>
    {
        public async Task Handle(BookingReservedDomainEvent notification, CancellationToken cancellationToken)
        {
            var booking = await bookingRepository.GetByIdAsync(notification.BookingId, cancellationToken);
            if (booking is null)
                return;

            var user = await userRepository.GetByIdAsync(booking.UserId, cancellationToken);
            if (user is null)
                return;

            var subject = "Booking Reserved";
            var body = $"You have 10 minutes to confirm this booking";

            await emailService.SendEmailAsync(user.Email, subject, body);
        }
    }
}