namespace Bookify.Application.Bookings.Dtos
{
    public class AddressDto
    {
        public string Country { get; init; }
        public string City { get; init; }
        public string State { get; init; }
        public string ZipCode { get; init; }
        public string Street { get; init; }
    }
}