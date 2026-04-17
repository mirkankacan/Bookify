using Bookify.Domain.Abstractions;
using Bookify.Infrastructure.Data;

namespace Bookify.Infrastructure.Repositories
{
    internal abstract class Repository<T>(ApplicationDbContext context) where T : Entity
    {
        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await context.Set<T>().FindAsync(new object[] { id }, cancellationToken);
        }

        public virtual void Add(T entity)
        {
            context.Add(entity);
        }
    }
}