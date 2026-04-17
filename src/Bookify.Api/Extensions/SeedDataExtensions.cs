using Bogus;
using Bookify.Application.Abstractions.Data;
using Dapper;
using System.Data;

namespace Bookify.Api.Extensions
{
    internal static class SeedDataExtensions
    {
        private static readonly string[] Currencies = ["USD", "EUR", "GBP", "TRY"];

        public static void SeedData(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            var sqlFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
            using IDbConnection connection = sqlFactory.CreateConnection();

            var existingCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM users");
            if (existingCount > 0) return;

            var faker = new Faker();

            var userIds = SeedUsers(connection, faker);
            var apartmentIds = SeedApartments(connection, faker);
            SeedBookings(connection, faker, apartmentIds, userIds);
        }

        private static List<Guid> SeedUsers(IDbConnection connection, Faker faker)
        {
            var users = Enumerable.Range(0, 100).Select(i =>
            {
                var firstName = faker.Name.FirstName();
                var lastName = faker.Name.LastName();
                return new
                {
                    Id = Guid.NewGuid(),
                    FirstName = firstName,
                    LastName = lastName,
                    Email = $"{firstName.ToLower()}.{lastName.ToLower()}{i}@bookify.com"
                };
            }).ToList();

            const string sql = """
                INSERT INTO users (id, first_name, last_name, email)
                VALUES (@Id, @FirstName, @LastName, @Email)
                """;

            connection.Execute(sql, users);

            return users.Select(u => u.Id).ToList();
        }

        private static List<Guid> SeedApartments(IDbConnection connection, Faker faker)
        {
            var allAmenities = Enumerable.Range(1, 10).ToArray();

            var apartments = Enumerable.Range(0, 100).Select(_ =>
            {
                var currency = faker.PickRandom(Currencies);
                var amenityCount = faker.Random.Int(1, 5);
                var amenities = faker.PickRandom(allAmenities, amenityCount).ToArray();

                return new
                {
                    Id = Guid.NewGuid(),
                    Name = faker.Company.CompanyName() + " Apartments",
                    Country = faker.Address.Country(),
                    City = faker.Address.City(),
                    State = faker.Address.State(),
                    ZipCode = faker.Address.ZipCode(),
                    Street = faker.Address.StreetAddress(),
                    Description = faker.Lorem.Paragraph(),
                    PriceAmount = Math.Round(faker.Random.Decimal(50, 500), 2),
                    PriceCurrency = currency,
                    CleaningFeeAmount = Math.Round(faker.Random.Decimal(10, 100), 2),
                    CleaningFeeCurrency = currency,
                    LastBookedOnUtc = (DateTime?)null,
                    Amenities = amenities
                };
            }).ToList();

            const string sql = """
                INSERT INTO apartments (
                    id, name,
                    address_country, address_city, address_state, address_zip_code, address_street,
                    description,
                    price_amount, price_currency,
                    cleaning_fee_amount, cleaning_fee_currency,
                    last_booked_on_utc, amenities)
                VALUES (
                    @Id, @Name,
                    @Country, @City, @State, @ZipCode, @Street,
                    @Description,
                    @PriceAmount, @PriceCurrency,
                    @CleaningFeeAmount, @CleaningFeeCurrency,
                    @LastBookedOnUtc, @Amenities)
                """;

            connection.Execute(sql, apartments);

            return apartments.Select(a => a.Id).ToList();
        }

        private static void SeedBookings(
            IDbConnection connection,
            Faker faker,
            List<Guid> apartmentIds,
            List<Guid> userIds)
        {
            var statuses = Enum.GetValues<BookingStatusSeed>();

            var bookings = Enumerable.Range(0, 100).Select(_ =>
            {
                var currency = faker.PickRandom(Currencies);
                var startDate = DateOnly.FromDateTime(faker.Date.Past(1));
                var endDate = startDate.AddDays(faker.Random.Int(1, 14));
                var status = faker.PickRandom(statuses);

                var priceForPeriod = Math.Round(faker.Random.Decimal(100, 2000), 2);
                var cleaningFee = Math.Round(faker.Random.Decimal(10, 100), 2);
                var amenitiesUpCharge = Math.Round(faker.Random.Decimal(0, 50), 2);
                var totalPrice = priceForPeriod + cleaningFee + amenitiesUpCharge;

                var createdOnUtc = DateTime.SpecifyKind(faker.Date.Past(1), DateTimeKind.Utc);

                return new
                {
                    Id = Guid.NewGuid(),
                    ApartmentId = faker.PickRandom(apartmentIds),
                    UserId = faker.PickRandom(userIds),
                    DurationStart = startDate,
                    DurationEnd = endDate,
                    PriceForPeriodAmount = priceForPeriod,
                    PriceForPeriodCurrency = currency,
                    CleaningFeeAmount = cleaningFee,
                    CleaningFeeCurrency = currency,
                    AmentitiesUpChargeAmount = amenitiesUpCharge,
                    AmentitiesUpChargeCurrency = currency,
                    TotalPriceAmount = totalPrice,
                    TotalPriceCurrency = currency,
                    Status = (int)status,
                    CreatedOnUtc = createdOnUtc,
                    ConfirmedOnUtc = status == BookingStatusSeed.Confirmed
                        ? DateTime.SpecifyKind(faker.Date.Between(createdOnUtc, DateTime.UtcNow), DateTimeKind.Utc)
                        : (DateTime?)null,
                    RejectedOnUtc = status == BookingStatusSeed.Rejected
                        ? DateTime.SpecifyKind(faker.Date.Between(createdOnUtc, DateTime.UtcNow), DateTimeKind.Utc)
                        : (DateTime?)null,
                    CompletedOnUtc = status == BookingStatusSeed.Completed
                        ? DateTime.SpecifyKind(faker.Date.Between(createdOnUtc, DateTime.UtcNow), DateTimeKind.Utc)
                        : (DateTime?)null,
                    CancelledOnUtc = status == BookingStatusSeed.Cancelled
                        ? DateTime.SpecifyKind(faker.Date.Between(createdOnUtc, DateTime.UtcNow), DateTimeKind.Utc)
                        : (DateTime?)null
                };
            }).ToList();

            const string sql = """
                INSERT INTO bookings (
                    id, apartment_id, user_id,
                    duration_start, duration_end,
                    price_for_period_amount, price_for_period_currency,
                    cleaning_fee_amount, cleaning_fee_currency,
                    amentities_up_charge_amount, amentities_up_charge_currency,
                    total_price_amount, total_price_currency,
                    status, created_on_utc,
                    confirmed_on_utc, rejected_on_utc, completed_on_utc, cancelled_on_utc)
                VALUES (
                    @Id, @ApartmentId, @UserId,
                    @DurationStart, @DurationEnd,
                    @PriceForPeriodAmount, @PriceForPeriodCurrency,
                    @CleaningFeeAmount, @CleaningFeeCurrency,
                    @AmentitiesUpChargeAmount, @AmentitiesUpChargeCurrency,
                    @TotalPriceAmount, @TotalPriceCurrency,
                    @Status, @CreatedOnUtc,
                    @ConfirmedOnUtc, @RejectedOnUtc, @CompletedOnUtc, @CancelledOnUtc)
                """;

            connection.Execute(sql, bookings);
        }

        private enum BookingStatusSeed
        {
            Reserved = 1,
            Confirmed = 2,
            Rejected = 3,
            Completed = 4,
            Cancelled = 5
        }
    }
}
