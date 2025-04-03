using AutoMapper;
using CollegeEvent.API.Dtos.RSO;
using CollegeEvent.API.Models;
using CollegeEvent.API.Repositories;
using CollegeEvent.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollegeEvent.API.Controller;

[Route("api/rso")]
[ApiController]
[Authorize]
[Authorize(Policy = "UniversityFound")]
public class RSOController(
	IRSORepository rsoRepository,
	IMapper mapper,
	JwtTokenService jwtTokenService,
	IUserRepository userRepository,
	IUniversityRepository universityRepository
) : ControllerBase
{
	private readonly IRSORepository rsoRepository = rsoRepository;
	private readonly IMapper mapper = mapper;
	private readonly JwtTokenService jwtTokenService = jwtTokenService;
	private readonly IUserRepository userRepository = userRepository;
	private readonly IUniversityRepository universityRepository = universityRepository;

	[HttpPost]
	[Authorize(Policy = "StudentAdminPolicy")]
	public async Task<IActionResult> AddRso([FromBody] AddRSORequest addRSORequest)
	{
		int? userId = this.jwtTokenService.GetUserIdFromClaims(HttpContext.User.Claims.ToList());

		if (userId == null)
		{
			return Unauthorized();
		}

		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}


		var rso = this.mapper.Map<RSO>(addRSORequest);

		var user = await this.userRepository.GetById((int)userId);

		if (user == null)
		{
			return Unauthorized();
		}

		var university = await this.universityRepository.GetById((int)user.UniversityID!);

		if (university == null)
		{
			return BadRequest();
		}

		// Verify email existence and domain
		var emailVerifyTasks = new List<Task<User?>>();

		foreach (var email in addRSORequest.MemberEmails)
		{
			emailVerifyTasks.Add(this.userRepository.GetByEmail(email));
		}

		foreach (var res in await Task.WhenAll(emailVerifyTasks))
		{
			if (res == null || res.Email.Split("@").Last() != university.Domain || res.UID == userId)
			{
				return BadRequest();
			}
		}

		rso = await this.rsoRepository.Create((int)userId!, rso, addRSORequest.MemberEmails);

		if (rso == null)
		{
			return BadRequest();
		}

		// Update user role to admin if user is a student
		if (user != null && user.UserRole == "Student")
		{
			await this.userRepository.UpdateUserRole((int)userId, "Admin");
		}

		var response = this.mapper.Map<RSOResponse>(rso);

		return CreatedAtAction(nameof(GetRso), new { id = response.RSOID }, response);
	}

	[HttpGet]
	[Route("{id}")]
	public async Task<IActionResult> GetRso([FromRoute] int id)
	{
		var foundRso = await this.rsoRepository.GetById(id);

		if (foundRso == null)
		{
			return NotFound();
		}

		var response = this.mapper.Map<RSOResponse>(foundRso);

		return Ok(response);
	}

	[HttpPut]
	[Route("{id}")]
	[Authorize(Policy = "StudentAdminPolicy")]
	public async Task<IActionResult> UpdateRso([FromRoute] int id, [FromBody] UpdateRSORequest updateRSORequest)
	{
		int? userId = this.jwtTokenService.GetUserIdFromClaims(HttpContext.User.Claims.ToList());

		if (userId == null)
		{
			return Unauthorized();
		}

		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		var rso = this.mapper.Map<RSO>(updateRSORequest);

		// Check if user permission
		if (rso.AdminID != userId)
		{
			return Unauthorized();
		}

		// Update RSO
		rso = await this.rsoRepository.Update(id, rso);

		if (rso == null)
		{
			return BadRequest();
		}

		var response = this.mapper.Map<RSOResponse>(rso);

		return Ok(response);
	}

	[HttpDelete]
	[Route("{id}")]
	[Authorize(Policy = "StudentAdminPolicy")]
	public async Task<IActionResult> DeleteRso([FromRoute] int id)
	{
		int? userId = this.jwtTokenService.GetUserIdFromClaims(HttpContext.User.Claims.ToList());

		if (userId == null)
		{
			return Unauthorized();
		}

		var rso = await this.rsoRepository.GetById(id);

		if (rso == null)
		{
			return NotFound();
		}

		// Check if user permission
		if (rso.AdminID != userId)
		{
			return Unauthorized();
		}

		await this.rsoRepository.Delete(id);

		return NoContent();
	}

	[HttpGet]
	[Route("own")]
	[Authorize(Policy = "StudentAdminPolicy")]
	public async Task<IActionResult> GetOwnedRsos()
	{
		int? userId = this.jwtTokenService.GetUserIdFromClaims(HttpContext.User.Claims.ToList());

		if (userId == null)
		{
			return Unauthorized();
		}

		var rsos = await this.rsoRepository.GetAllByAdminId((int)userId);

		return Ok(rsos);
	}

	[HttpGet]
	[Route("joined")]
	public async Task<IActionResult> GetJoinedRsos()
	{
		int? userId = this.jwtTokenService.GetUserIdFromClaims(HttpContext.User.Claims.ToList());

		if (userId == null)
		{
			return Unauthorized();
		}

		var rsos = await this.rsoRepository.GetAllByStudentId((int)userId);

		return Ok(rsos);
	}

	[HttpGet]
	[Route("join")]
	public async Task<IActionResult> GetAvailableRsos()
	{
		int? userId = this.jwtTokenService.GetUserIdFromClaims(HttpContext.User.Claims.ToList());

		if (userId == null)
		{
			return Unauthorized();
		}

		var rsos = await this.rsoRepository.GetAllByAvailability((int)userId);

		return Ok(rsos);
	}

	[HttpPost]
	[Route("join/{rsoId}")]
	public async Task<IActionResult> JoinRso([FromRoute] int rsoId)
	{
		int? userId = this.jwtTokenService.GetUserIdFromClaims(HttpContext.User.Claims.ToList());

		// Check if users exists
		if (userId == null)
		{
			return Unauthorized();
		}

		// Check if RSO exists
		var rso = this.rsoRepository.GetById(rsoId);

		if (rso == null)
		{
			return NotFound();
		}

		var success = await this.rsoRepository.CreateRsoMembers((int)userId, rsoId);

		if (!success)
		{
			return BadRequest();
		}

		return Ok();
	}

	[HttpPost]
	[Route("leave/{rsoId}")]
	public async Task<IActionResult> LeaveRso([FromRoute] int rsoId)
	{
		int? userId = this.jwtTokenService.GetUserIdFromClaims(HttpContext.User.Claims.ToList());

		// Check if users exists
		if (userId == null)
		{
			return Unauthorized();
		}

		// Check if RSO exists
		var rso = this.rsoRepository.GetById(rsoId);

		if (rso == null)
		{
			return NotFound();
		}

		var success = await this.rsoRepository.DeleteRsoMembers((int)userId, rsoId);

		if (!success)
		{
			return BadRequest();
		}

		return Ok();
	}
}