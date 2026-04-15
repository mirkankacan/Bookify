using FluentValidation;

namespace Bookify.Application.Bookings.ReserveBooking
{
    public class ReserveBookingCommandValidator : AbstractValidator<ReserveBookingCommand>
    {
        public ReserveBookingCommandValidator()
        {
            RuleFor(x => x.ApartmentId)
                .NotEmpty().WithMessage("Apartment ID is required");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required")
                .LessThanOrEqualTo(x => x.EndDate).WithMessage("Start date must be before or equal end date");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required")
                .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be after or equal start date");
        }
    }
}