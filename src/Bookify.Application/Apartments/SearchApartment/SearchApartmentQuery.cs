using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Apartments.Dtos;

namespace Bookify.Application.Apartments.SearchApartment
{
    public sealed record SearchApartmentQuery(DateOnly StartDate, DateOnly EndDate) : IQuery<IReadOnlyList<ApartmentDto>>;
}