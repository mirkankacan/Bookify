using Bookify.Domain.Apartments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Data.Configuration
{
    public class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
    {
        public void Configure(EntityTypeBuilder<Apartment> builder)
        {
            builder.ToTable("apartments");
            builder.HasKey(x => x.Id);
            builder.OwnsOne(x => x.Address);
            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .HasConversion(x => x.Value, value => new Name(value));

            builder.Property(x => x.Description)
              .HasMaxLength(2000)
              .HasConversion(x => x.Value, value => new Description(value));

            builder.OwnsOne(x => x.Price, priceBuilder =>
            {
                priceBuilder.Property(money => money.Currency)
                .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
            });
            builder.OwnsOne(x => x.CleaningFee, priceBuilder =>
            {
                priceBuilder.Property(money => money.Currency)
                .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
            });
            builder.Property<uint>("Version").IsRowVersion();
        }
    }
}