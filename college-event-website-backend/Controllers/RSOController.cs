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
	IUserRepository userRepository
) : ControllerBase
{
	private readonly IRSORepository rsoRepository = rsoRepository;
	private readonly IMapper mapper = mapper;
	private readonly JwtTokenService jwtTokenService = jwtTokenService;
	private readonly IUserRepository userRepository = userRepository;

	[HttpPost]
	[Authorize(Policy = "AdminPolicy")]
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

		rso = await this.rsoRepository.Create((int)userId!, rso);

		if (rso == null)
		{
			return BadRequest();
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

		return Ok(foundRso);
	}

	[HttpPut]
	[Route("{id}")]
	[Authorize(Policy = "AdminPolicy")]
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
	[Authorize(Policy = "AdminPolicy")]
	public async Task<IActionResult> DeleteRso([FromRoute] int id)
	{
		int? userId = this.jwtTokenService.GetUserIdFromClaims(HttpContext.User.Claims.ToList());

		if (userId == null)
		{
			return Unauthorized();
		}

		var rso = await this.rsoRepository.Delete(id);

		if (rso == null)
		{
			return NotFound();
		}

		return NoContent();
	}

	[HttpGet]
	[Route("own")]
	[Authorize(Policy = "AdminPolicy")]
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
	[Authorize(Policy = "StudentPolicy")]
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

	[HttpPost]
	[Route("join/{rsoId}")]
	[Authorize(Policy = "StudentPolicy")]
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
	[Authorize(Policy = "StudentPolicy")]
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