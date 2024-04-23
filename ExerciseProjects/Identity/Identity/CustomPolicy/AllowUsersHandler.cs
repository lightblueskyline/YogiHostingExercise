using Microsoft.AspNetCore.Authorization;

namespace Identity.CustomPolicy
{
    /// <summary>
    /// Custom Requirement to an Identity Policy
    /// </summary>
    public class AllowUsersHandler : AuthorizationHandler<AllowUserPolicy>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AllowUserPolicy requirement)
        {
            if ((requirement.AllowUsers ?? new string[] { }).Any(x => x.Equals(context?.User?.Identity?.Name, StringComparison.OrdinalIgnoreCase)))
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
