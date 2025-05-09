using AutoMapper;
using CollegeEvent.API.Dtos.Auth;
using CollegeEvent.API.Models;
using CollegeEvent.API.Repositories;
using CollegeEvent.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollegeEvent.API.Controller;

[Route("api/auth")]
[ApiController]
public class AuthController(
	IUserRepository userRepository,
	IUniversityRepository universityRepository,
	PasswordHashService passwordHashService,
	JwtTokenService jwtTokenService,
	IConfiguration configuration,
	IMapper mapper) : ControllerBase
{
	private readonly IUserRepository userRepository = userRepository;
	private readonly IUniversityRepository universityRepository = universityRepository;
	private readonly PasswordHashService passwordHashService = passwordHashService;
	private readonly JwtTokenService jwtTokenService = jwtTokenService;
	private readonly IConfiguration configuration = configuration;
	private readonly IMapper mapper = mapper;

	[Route("sign-up")]
	[HttpPost]
	public async Task<IActionResult> SignUp([FromBody] SignUpRequest signUpRequest)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		var user = new User()
		{
			Email = signUpRequest.Email,
			Name = signUpRequest.Name,
			PasswordHash = passwordHashService.HashPassword(signUpRequest.Password),
			UserRole = signUpRequest.UserRole
		};

		var university = await this.universityRepository.GetUniversityByDomain(user.Email.Split("@").Last());

		// Check if Email domain is valid unless the user is a super admin
		if (user.UserRole != "SuperAdmin" && university == null)
		{
			ModelState.AddModelError("UniversityNotFound", "The Email domain does not belong to a registered university");
			return NotFound(ModelState);
		}

		user.UniversityID = university?.UniversityID;

		// Add user to DB
		user = await this.userRepository.Create(user);

		if (user == null)
		{
			return BadRequest();
		}

		return Ok();
	}

	[Route("login")]
	[HttpPost]
	public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		var user = await this.userRepository.Authenticate(loginRequest.Email, loginRequest.Password);

		if (user == null)
		{
			return Unauthorized();
		}

		var token = this.jwtTokenService.Create(user);

		HttpContext.Response.Cookies.Append("X-Access-Token", token, new CookieOptions()
		{
			HttpOnly = true,
			Secure = true,
			SameSite = SameSiteMode.None,
			Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes"))
		});

		return Ok();
	}

	[Route("user")]
	[HttpGet]
	[Authorize]
	public async Task<IActionResult> GetUser()
	{
		int? uid = this.jwtTokenService.GetUserIdFromClaims(HttpContext.User.Claims.ToList());

		// Check if claim user ID exists
		if (uid == null)
		{
			return Unauthorized();
		}

		User? foundUser = await this.userRepository.GetById((int)uid);

		if (foundUser == null)
		{
			return NotFound();
		}

		var response = this.mapper.Map<UserResponse>(foundUser);

		return Ok(response);
	}

	[Route("logout")]
	[HttpPost]
	public IActionResult Logout()
	{
		HttpContext.Response.Cookies.Delete("X-Access-Token");
		HttpContext.Response.Cookies.Delete("X-Refresh-Token");

		return Ok();
	}
}
