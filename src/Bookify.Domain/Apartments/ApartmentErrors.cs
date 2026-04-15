using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Apartments
{
    public static class ApartmentErrors
    {
        public static readonly Error ApartmentNotFound = new Error("Apartment.NotFound", "Apartment not found");
    }
}