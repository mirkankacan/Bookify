using Bookify.Domain.Apartments;
using Bookify.Infrastructure.Data;

namespace Bookify.Infrastructure.Repositories
{
    internal class ApartmentRepository : Repository<Apartment>, IApartmentRepository
    {
        public ApartmentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}