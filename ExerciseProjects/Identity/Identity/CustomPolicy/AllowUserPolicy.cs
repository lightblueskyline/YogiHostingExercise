using Microsoft.AspNetCore.Authorization;

namespace Identity.CustomPolicy
{
    /// <summary>
    /// Custom Requirement to an Identity Policy
    /// </summary>
    public class AllowUserPolicy : IAuthorizationRequirement
    {
        public string[]? AllowUsers { get; set; }

        public AllowUserPolicy(params string[] users)
        {
            // params 关键字用于声明一个方法参数数组
            AllowUsers = users;
        }
    }
}
