using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Apartments.Dtos;
using Bookify.Application.Bookings.Dtos;
using Bookify.Domain.Bookings;
using Dapper;

namespace Bookify.Application.Apartments.SearchApartment
{
    internal sealed class SearchApartmentQueryHandler(ISqlConnectionFactory factory) : IQueryHandler<SearchApartmentQuery, IReadOnlyList<ApartmentDto>>
    {
        private static readonly BookingStatus[] ActiveBookingStatuses =
        {
            BookingStatus.Reserved,
            BookingStatus.Confirmed,
            BookingStatus.Completed
        };

        public async Task<IReadOnlyList<ApartmentDto>> Handle(SearchApartmentQuery request, CancellationToken cancellationToken)
        {
            if (request.StartDate > request.EndDate)
            {
                return new List<ApartmentDto>();
            }
            using var connection = factory.CreateConnection();
            const string sql = @"SELECT
    a.id AS Id,
    a.name AS Name,
    a.description AS Description,
    a.price_amount AS Price,
    a.price_currency AS Currency,
    a.address_country AS Country,
    a.address_state AS State,
    a.address_zip_code AS ZipCode,
    a.address_city AS City,
    a.address_street AS Street
FROM apartments AS a
WHERE NOT EXISTS
(
    SELECT 1
    FROM bookings AS b
    WHERE
        b.apartment_id = a.id AND
        b.duration_start <= @EndDate AND
        b.duration_end >= @StartDate AND
        b.status = ANY(@ActiveBookingStatuses)
)";
            var apartments = await connection.QueryAsync<ApartmentDto, AddressDto, ApartmentDto>(sql, (apartment, address) =>
            {
                apartment.Address = address;
                return apartment;
            },
            new
            {
                request.StartDate,
                request.EndDate,
                ActiveBookingStatuses = ActiveBookingStatuses.Cast<int>().ToArray()
            },
            splitOn: "Country");

            return apartments.ToList();
        }
    }
}