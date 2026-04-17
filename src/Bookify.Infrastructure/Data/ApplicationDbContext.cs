using Bookify.Application.Exceptions;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Bookify.Infrastructure.Data
{
    public sealed class ApplicationDbContext : DbContext, IUnitOfWork
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        private readonly IPublisher _publisher;

        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Apartment> Apartments { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPublisher publisher) : base(options)
        {
            _publisher = publisher;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        private async Task PublishDomainEventsAsync(CancellationToken cancellationToken = default)
        {
            var domainEntities = ChangeTracker
                .Entries<Entity>()
                .Select(e => e.Entity)
                .SelectMany(entry =>
                {
                    var domainEvents = entry.GetDomainEvents();
                    entry.ClearDomainEvents();
                    return domainEvents;
                }).ToList();
            foreach (var domainEntity in domainEntities)
            {
                await _publisher.Publish(domainEntity, cancellationToken);
            }
        }

        public async Task<int> SaveChangesWithDomainEventsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await PublishDomainEventsAsync(cancellationToken);
                var result = await SaveChangesAsync(cancellationToken);
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine(ex);
                throw new ConcurrencyException("Concurrency exception occured", ex);
            }
        }
    }
}