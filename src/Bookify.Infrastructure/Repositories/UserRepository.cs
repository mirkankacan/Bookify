using Bookify.Domain.Users;
using Bookify.Infrastructure.Data;

namespace Bookify.Infrastructure.Repositories
{
    internal class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}