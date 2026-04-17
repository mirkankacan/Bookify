using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Apartments
{
    public sealed class Apartment : Entity
    {
        private Apartment() : base(Guid.Empty) { }

        public Apartment(Guid id, Name name, Address address, Description description, Money price, Money cleaningFee, DateTime lastBookedOnUtc, List<Amenity> amenities) : base(id)
        {
            Name = name;
            Address = address;
            Description = description;
            Price = price;
            CleaningFee = cleaningFee;
            LastBookedOnUtc = lastBookedOnUtc;
            Amenities = amenities;
        }

        public Name Name { get; private set; }
        public Address Address { get; private set; }
        public Description Description { get; private set; }
        public Money Price { get; private set; }
        public Money CleaningFee { get; private set; }
        public DateTime? LastBookedOnUtc { get; internal set; }
        public List<Amenity> Amenities { get; private set; } = new List<Amenity>();
    }
}