using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Bookify.Infrastructure.Authorization
{
    internal sealed class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;

        public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
            _options = options.Value;
        }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);
            if (policy is not null)
                return policy;

            var permissionPolicy = new AuthorizationPolicyBuilder()
                .AddRequirements(new HasPermissionRequirement(policyName))
                .Build();

            _options.AddPolicy(policyName, permissionPolicy);
            return permissionPolicy;
        }
    }
}