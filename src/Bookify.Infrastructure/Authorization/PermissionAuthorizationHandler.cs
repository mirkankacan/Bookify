using Bookify.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Infrastructure.Authorization
{
    public sealed class PermissionAuthorizationHandler(IServiceProvider sp) : AuthorizationHandler<HasPermissionRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionRequirement requirement)
        {
            if (context.User?.Identity?.IsAuthenticated != true)
                return;

            using var scope = sp.CreateScope();
            var permissionService = scope.ServiceProvider.GetRequiredService<AuthorizationService>();
            var identityId = context.User.GetIdentityId();
            var permissions = await permissionService.GetPermissionsForUserAsync(identityId);

            if (permissions.Contains(requirement.Permission))
                context.Succeed(requirement);
        }
    }
}