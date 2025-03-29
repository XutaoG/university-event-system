using CollegeEvent.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using CollegeEvent.API.Dtos.University;
using CollegeEvent.API.Models;
using AutoMapper;
using CollegeEvent.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace CollegeEvent.API.Controller;

[Route("api/university")]
[ApiController]
[Authorize]
public class UniversityController(
	IUniversityRepository universityRepository,
	IUserRepository userRepository,
	IMapper mapper,
	JwtTokenService jwtTokenService) : ControllerBase
{
	private readonly IUniversityRepository universityRepository = universityRepository;
	private readonly IMapper mapper = mapper;
	private readonly JwtTokenService jwtTokenService = jwtTokenService;
	private readonly IUserRepository userRepository = userRepository;

	[HttpPost]
	[Authorize(Policy = "SuperAdminPolicy")]
	public async Task<IActionResult> AddUniversity([FromBody] AddUniversityRequest addUniversityRequest)
	{
		int? userId = this.jwtTokenService.GetUserIdFromClaims(HttpContext.User.Claims.ToList());

		var user = await this.userRepository.GetById((int)userId!);

		// Check if a university is already created for this super admin
		if (user == null || user.UniversityID != null)
		{
			return Conflict();
		}

		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		var university = this.mapper.Map<University>(addUniversityRequest);

		// Set university domain
		university.Domain = user.Email.Split("@").Last();

		university = await this.universityRepository.Create(university);

		if (university == null)
		{
			return BadRequest();
		}

		var response = this.mapper.Map<UniversityResponse>(university);

		// Assign university to user
		await this.userRepository.AssignUniversity((int)userId!, response.UniversityID);

		return CreatedAtAction(nameof(GetUniversity), new { universityID = response.UniversityID, response });
	}

	[HttpPut]
	[Authorize(Policy = "SuperAdminPolicy")]
	public async Task<IActionResult> UpdateUniversity([FromBody] UpdateUniversityRequest updateUniversityRequest)
	{
		int? userId = this.jwtTokenService.GetUserIdFromClaims(HttpContext.User.Claims.ToList());

		var user = await this.userRepository.GetById((int)userId!);

		// Check if uniID is valid
		if (user == null || user.UniversityID == null)
		{
			return NotFound();
		}

		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		var university = this.mapper.Map<University>(updateUniversityRequest);

		// Update uni
		university = await this.universityRepository.Update((int)user.UniversityID, university);

		if (university == null)
		{
			return BadRequest();
		}

		var response = this.mapper.Map<UniversityResponse>(university);

		return Ok(response);
	}

	[HttpGet]
	[Route("{id}")]
	public async Task<IActionResult> GetUniversity([FromRoute] int id)
	{
		var foundUniversity = await this.universityRepository.GetById(id);

		if (foundUniversity == null)
		{
			return NotFound();
		}

		return Ok(foundUniversity);
	}
}