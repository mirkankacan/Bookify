using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Users;

namespace Bookify.Application.Bookings.ReserveBooking
{
    internal sealed class ReserveBookingCommandHandler(IUserRepository userRepository, IApartmentRepository apartmentRepository, IBookingRepository bookingRepository, IUnitOfWork unitOfWork, PricingService pricingService, IDateTimeProvider dateTimeProvider) : ICommandHandler<ReserveBookingCommand, Guid>

    {
        public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
                return Result.Failure<Guid>(UserErrors.UserNotFound);

            var apartment = await apartmentRepository.GetByIdAsync(request.ApartmentId, cancellationToken);
            if (apartment is null)
                return Result.Failure<Guid>(ApartmentErrors.ApartmentNotFound);

            var duration = DateRange.Create(request.StartDate, request.EndDate);
            if (await bookingRepository.IsOverlappingAsync(apartment, duration, cancellationToken))
                return Result.Failure<Guid>(BookingErrors.Overlap);

            var booking = Booking.Reserve(apartment, request.UserId, duration, dateTimeProvider.UtcNow, pricingService);
            bookingRepository.Add(booking);
            await unitOfWork.SaveChangesAsync();
            return Result.Success(booking.Id);
        }
    }
}