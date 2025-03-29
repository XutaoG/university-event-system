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
	PasswordHashService passwordHashService,
	JwtTokenService jwtTokenService,
	IConfiguration configuration,
	IMapper mapper) : ControllerBase
{
	private readonly IUserRepository userRepository = userRepository;
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
}
