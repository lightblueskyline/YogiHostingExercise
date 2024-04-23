using Microsoft.AspNetCore.Authorization;

namespace Identity.CustomPolicy
{
    /// <summary>
    /// Apply Policy without [Authorize] attribute
    /// </summary>
    public class AllowPrivatePolicy : IAuthorizationRequirement
    {
    }

    /// <summary>
    /// Apply Policy without [Authorize] attribute
    /// </summary>
    public class AllowPrivateHandler : AuthorizationHandler<AllowPrivatePolicy>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AllowPrivatePolicy requirement)
        {
            string[]? allowedUsers = context.Resource as string[];
            if ((allowedUsers ?? new string[] { }).Any(x => x.Equals(context?.User?.Identity?.Name, StringComparison.OrdinalIgnoreCase)))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
