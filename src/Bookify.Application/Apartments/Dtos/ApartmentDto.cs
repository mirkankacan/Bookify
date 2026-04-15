using Bookify.Application.Bookings.Dtos;

namespace Bookify.Application.Apartments.Dtos
{
    public class ApartmentDto
    {
        public Guid Id { get; init; }

        public string Name { get; init; }

        public AddressDto Address { get; set; }
        public string Description { get; init; }
        public decimal Price { get; init; }
        public decimal CleaningFee { get; init; }
    }
}