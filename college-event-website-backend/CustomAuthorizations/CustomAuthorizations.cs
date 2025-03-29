using CollegeEvent.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace CollegeEvent.API.CustomAuthorizations;

public class UserRoleAuthorizationRequirement(string requiredRole) : IAuthorizationRequirement
{
	public string RequiredRole { get; } = requiredRole;
}

public class UserRoleAuthorizationHandler : AuthorizationHandler<UserRoleAuthorizationRequirement>
{

	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRoleAuthorizationRequirement requirement)
	{
		// Get user role
		var userRoleClaim = context.User?.FindFirst("userRole");

		// If claim has User Role
		if (userRoleClaim != null && userRoleClaim.Value == requirement.RequiredRole)
		{
			context.Succeed(requirement);
		}

		return Task.CompletedTask;
	}
}