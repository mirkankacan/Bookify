using Bookify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Authorization
{
    internal sealed class AuthorizationService(ApplicationDbContext dbContext)
    {
        public async Task<UserRolesDto> GetRolesForUserAsync(string identityId, CancellationToken cancellationToken = default)
        {
            var roles = await dbContext.Users
                .Where(u => u.IdentityId == identityId)
                .Select(u => new UserRolesDto(u.Id, u.Roles.ToList()))
                .FirstAsync(cancellationToken);

            return roles;
        }

        public async Task<HashSet<string>> GetPermissionsForUserAsync(string identityId, CancellationToken cancellationToken = default)
        {
            var perms = await dbContext.Users
                .Where(u => u.IdentityId == identityId)
                .SelectMany(u => u.Roles)
                .SelectMany(r => r.Permissions)
                .ToListAsync(cancellationToken);

            var permsSet = perms.Select(p => p.Name).ToHashSet() ?? new HashSet<string>();
            return permsSet;
        }
    }
}