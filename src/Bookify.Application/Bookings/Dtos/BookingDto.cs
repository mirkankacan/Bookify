using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;

namespace Bookify.Application.Bookings.Dtos
{
    public class BookingDto
    {
        public Guid Id { get; init; }
        public Guid ApartmentId { get; init; }
        public Guid UserId { get; init; }
        public DateRange Duration { get; init; }
        public Money PriceForPeriod { get; init; }
        public Money CleaningFee { get; init; }
        public Money AmentitiesUpCharge { get; init; }
        public Money TotalPrice { get; init; }
        public BookingStatus Status { get; init; }

        public DateTime CreatedOnUtc { get; init; }
        public DateTime? ConfirmedOnUtc { get; init; }
        public DateTime? RejectedOnUtc { get; init; }
        public DateTime? CompletedOnUtc { get; init; }
        public DateTime? CancelledOnUtc { get; init; }
    }
}