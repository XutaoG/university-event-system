using CollegeEvent.API.Repositories;
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

public class UniversityFoundAuthorizationRequirement : IAuthorizationRequirement { }

public class UniversityFoundAuthorizationHandler(
	IUniversityRepository universityRepository,
	IUserRepository userRepository,
	JwtTokenService jwtTokenService) : AuthorizationHandler<UniversityFoundAuthorizationRequirement>
{
	private readonly IUniversityRepository universityRepository = universityRepository;
	private readonly IUserRepository userRepository = userRepository;
	private readonly JwtTokenService jwtTokenService = jwtTokenService;

	protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, UniversityFoundAuthorizationRequirement requirement)
	{
		int? userId = this.jwtTokenService.GetUserIdFromClaims(context.User.Claims.ToList());

		var user = await this.userRepository.GetById((int)userId!);

		// Check if uniID is valid
		if (user == null || user.UniversityID == null)
		{
			return;
		}

		var foundUniversity = await this.universityRepository.GetById((int)user.UniversityID);

		// Check if university is found
		if (foundUniversity == null)
		{
			return;
		}

		context.Succeed(requirement);
		return;
	}
}