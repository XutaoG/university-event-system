using AutoMapper;
using CollegeEvent.API.Dtos.Event;
using CollegeEvent.API.Dtos.Location;
using CollegeEvent.API.Models;
using CollegeEvent.API.Repositories;
using CollegeEvent.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollegeEvent.API.Controller;

[Route("api/event")]
[ApiController]
[Authorize]
public class EventController(
	JwtTokenService jwtTokenService,
	IMapper mapper,
	IEventRepository eventRepository,
	IRSORepository rsoRepository,
	ILocationRepository locationRepository,
	IUserRepository userRepository) : ControllerBase
{
	private readonly JwtTokenService jwtTokenService = jwtTokenService;
	private readonly IMapper mapper = mapper;
	private readonly IEventRepository eventRepository = eventRepository;
	private readonly IRSORepository rsoRepository = rsoRepository;
	private readonly ILocationRepository locationRepository = locationRepository;
	private readonly IUserRepository userRepository = userRepository;

	[Route("public")]
	[HttpPost]
	[Authorize(Policy = "AdminPolicy")]
	public async Task<IActionResult> AddPublicEvent([FromBody] AddPublicEventRequest addPublicEventRequest)
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

		// Get super admin ID
		var user = await this.userRepository.GetById((int)userId);

		if (user == null)
		{
			return Unauthorized();
		}

		var universityId = user.UniversityID;

		if (universityId == null)
		{
			return BadRequest();
		}

		// Add location
		var location = this.mapper.Map<Location>(addPublicEventRequest.Location);

		location = await this.locationRepository.Create(location);

		if (location == null)
		{
			return BadRequest();
		}

		// Add public event

		var publicEvent = this.mapper.Map<PublicEvent>(addPublicEventRequest);

		publicEvent.AdminID = (int)userId;
		publicEvent.LocID = location.LocID;
		publicEvent.UniversityID = (int)universityId;

		publicEvent = await this.eventRepository.CreatePublicEvent(publicEvent);

		if (publicEvent == null)
		{
			return BadRequest();
		}

		var locationResponse = this.mapper.Map<LocationResponse>(location);
		var eventResponse = this.mapper.Map<PublicEventResponse>(publicEvent);

		eventResponse.Location = locationResponse;

		return CreatedAtAction(nameof(GetPublicEvent), new { id = eventResponse.EventID }, eventResponse);
	}

	[Route("private")]
	[HttpPost]
	[Authorize(Policy = "AdminPolicy")]
	public async Task<IActionResult> AddPrivateEvent([FromBody] AddPrivateEventRequest addPrivateEventRequest)
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

		// Get super admin ID
		var user = await this.userRepository.GetById((int)userId);

		if (user == null)
		{
			return Unauthorized();
		}

		var universityId = user.UniversityID;

		if (universityId == null)
		{
			return BadRequest();
		}

		// Add location
		var location = this.mapper.Map<Location>(addPrivateEventRequest.Location);

		location = await this.locationRepository.Create(location);

		if (location == null)
		{
			return BadRequest();
		}

		// Add private event

		var privateEvent = this.mapper.Map<PrivateEvent>(addPrivateEventRequest);

		privateEvent.AdminID = (int)userId;
		privateEvent.LocID = location.LocID;
		privateEvent.UniversityID = (int)universityId;

		privateEvent = await this.eventRepository.CreatePrivateEvent(privateEvent);

		if (privateEvent == null)
		{
			return BadRequest();
		}

		var locationResponse = this.mapper.Map<LocationResponse>(location);
		var eventResponse = this.mapper.Map<PrivateEventResponse>(privateEvent);

		eventResponse.Location = locationResponse;

		return CreatedAtAction(nameof(GetPrivateEvent), new { id = eventResponse.EventID }, eventResponse);
	}

	[Route("rso/{id}")]
	[HttpPost]
	[Authorize(Policy = "AdminPolicy")]
	public async Task<IActionResult> AddRsoEvent([FromRoute] int id, [FromBody] AddRsoEventRequest addRsoEventRequest)
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

		// Get super admin ID
		var user = await this.userRepository.GetById((int)userId);

		if (user == null)
		{
			return Unauthorized();
		}

		// Check if RSO ID exists
		var rso = await this.rsoRepository.GetById(id);

		if (rso == null)
		{
			return NotFound();
		}

		// Add location
		var location = this.mapper.Map<Location>(addRsoEventRequest.Location);

		location = await this.locationRepository.Create(location);

		if (location == null)
		{
			return BadRequest();
		}

		// Add RSO event

		var rsoEvent = this.mapper.Map<RSOEvent>(addRsoEventRequest);

		rsoEvent.AdminID = (int)userId;
		rsoEvent.LocID = location.LocID;
		rsoEvent.RSOID = rso.RSOID;

		rsoEvent = await this.eventRepository.CreateRsoEvent(rsoEvent);

		if (rsoEvent == null)
		{
			return BadRequest();
		}

		var locationResponse = this.mapper.Map<LocationResponse>(location);
		var eventResponse = this.mapper.Map<RsoEventResponse>(rsoEvent);

		eventResponse.Location = locationResponse;

		return CreatedAtAction(nameof(GetPrivateEvent), new { id = eventResponse.EventID }, eventResponse);
	}

	[Route("public/{id}")]
	[HttpGet]
	public async Task<IActionResult> GetPublicEvent([FromRoute] int id)
	{
		var foundEvent = await this.eventRepository.GetPublicEventById(id);

		if (foundEvent == null)
		{
			return NotFound();
		}

		var eventResponse = this.mapper.Map<PublicEventResponse>(foundEvent);

		// Get location
		var foundLocation = await this.locationRepository.GetById(eventResponse.LocID);

		if (foundLocation == null)
		{
			return NotFound();
		}

		var locationResponse = this.mapper.Map<LocationResponse>(foundLocation);

		eventResponse.Location = locationResponse;

		return Ok(eventResponse);
	}

	[Route("private/{id}")]
	[HttpGet]
	public async Task<IActionResult> GetPrivateEvent([FromRoute] int id)
	{
		var foundEvent = await this.eventRepository.GetPrivateEventById(id);

		if (foundEvent == null)
		{
			return NotFound();
		}

		var eventResponse = this.mapper.Map<PrivateEventResponse>(foundEvent);

		// Get location
		var foundLocation = await this.locationRepository.GetById(eventResponse.LocID);

		if (foundLocation == null)
		{
			return NotFound();
		}

		var locationResponse = this.mapper.Map<LocationResponse>(foundLocation);

		eventResponse.Location = locationResponse;

		return Ok(eventResponse);
	}

	[Route("rso/{id}")]
	[HttpGet]
	public async Task<IActionResult> GetRsoEvent([FromRoute] int id)
	{
		var foundEvent = await this.eventRepository.GetRsoEventById(id);

		if (foundEvent == null)
		{
			return NotFound();
		}

		var eventResponse = this.mapper.Map<RsoEventResponse>(foundEvent);

		// Get location
		var foundLocation = await this.locationRepository.GetById(eventResponse.LocID);

		if (foundLocation == null)
		{
			return NotFound();
		}

		var locationResponse = this.mapper.Map<LocationResponse>(foundLocation);

		eventResponse.Location = locationResponse;

		return Ok(eventResponse);
	}

	[Route("{id}")]
	[HttpPut]
	[Authorize(Policy = "AdminPolicy")]
	public async Task<IActionResult> UpdateEvent([FromRoute] int id, [FromBody] UpdateEventRequest updateEventRequest)
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

		var event_ = await this.eventRepository.GetEventById(id);

		if (event_ == null)
		{
			return BadRequest();
		}

		// Check if user has permission
		if ((int)userId != event_.AdminID)
		{
			return Unauthorized();
		}

		event_ = this.mapper.Map<Event>(updateEventRequest);

		event_ = await this.eventRepository.UpdateEvent(id, event_);

		if (event_ == null)
		{
			return BadRequest();
		}

		var location = await this.locationRepository.GetById(event_.LocID);

		var locationResponse = this.mapper.Map<LocationResponse>(location);
		var eventResponse = this.mapper.Map<EventResponse>(event_);

		eventResponse.Location = locationResponse;

		return Ok(eventResponse);
	}

	[Route("{id}")]
	[HttpDelete]
	[Authorize(Policy = "AdminPolicy")]
	public async Task<IActionResult> DeleteEvent([FromRoute] int id)
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

		var event_ = await this.eventRepository.GetEventById(id);

		if (event_ == null)
		{
			return BadRequest();
		}

		// Check if user has permission
		if ((int)userId != event_.AdminID)
		{
			return Unauthorized();
		}

		event_ = await this.eventRepository.DeleteEvent(id);

		if (event_ == null)
		{
			return BadRequest();
		}

		var location = await this.locationRepository.Delete(event_.LocID);

		if (location == null)
		{
			return BadRequest();
		}

		return NoContent();
	}

	[Route("public/approve/{id}")]
	[HttpPost]
	[Authorize(Policy = "SuperAdminPolicy")]
	public async Task<IActionResult> ApprovePublicEvent([FromRoute] int id)
	{
		int? userId = this.jwtTokenService.GetUserIdFromClaims(HttpContext.User.Claims.ToList());

		if (userId == null)
		{
			return Unauthorized();
		}

		var publicEvent = await this.eventRepository.GetPublicEventById(id);

		if (publicEvent == null)
		{
			return NotFound();
		}

		var user = await this.userRepository.GetById((int)userId);

		// Check if super admin has permission to approve
		if (publicEvent.UniversityID != user!.UniversityID)
		{
			return Unauthorized();
		}

		publicEvent = await this.eventRepository.SetPublicEventApproved(publicEvent.EventID);

		return Ok(publicEvent);
	}

	[Route("public")]
	[HttpGet]
	public async Task<IActionResult> GetAllPublicEvents()
	{
		int? userId = this.jwtTokenService.GetUserIdFromClaims(HttpContext.User.Claims.ToList());

		if (userId == null)
		{
			return Unauthorized();
		}

		var publicEvents = await this.eventRepository.GetAllPublicEvents();

		var locationTasks = new List<Task<Location>>();

		for (int i = 0; i < publicEvents.Count; i++)
		{
			locationTasks.Add(this.locationRepository.GetById(publicEvents[i].LocID)!);
		}

		var response = this.mapper.Map<PublicEventResponse[]>(publicEvents);

		var locations = await Task.WhenAll(locationTasks);

		for (int i = 0; i < locations.Length; i++)
		{
			response[i].Location = this.mapper.Map<LocationResponse>(locations[i]);
		}

		return Ok(response);
	}

	[Route("private")]
	[HttpGet]
	public async Task<IActionResult> GetAllPrivateEvents()
	{
		int? userId = this.jwtTokenService.GetUserIdFromClaims(HttpContext.User.Claims.ToList());

		if (userId == null)
		{
			return Unauthorized();
		}

		var user = await this.userRepository.GetById((int)userId);

		if (user == null || user.UniversityID == null)
		{
			return Unauthorized();
		}

		var locationTasks = new List<Task<Location>>();

		var privateEvents = await this.eventRepository.GetAllPrivateEvents((int)user.UniversityID);

		for (int i = 0; i < privateEvents.Count; i++)
		{
			locationTasks.Add(this.locationRepository.GetById(privateEvents[i].LocID)!);
		}

		var response = this.mapper.Map<PrivateEventResponse[]>(privateEvents);

		var locations = await Task.WhenAll(locationTasks);

		for (int i = 0; i < locations.Length; i++)
		{
			response[i].Location = this.mapper.Map<LocationResponse>(locations[i]);
		}

		return Ok(response);
	}

	[Route("rso")]
	[HttpGet]
	public async Task<IActionResult> GetAllRsoEvents()
	{
		int? userId = this.jwtTokenService.GetUserIdFromClaims(HttpContext.User.Claims.ToList());

		if (userId == null)
		{
			return Unauthorized();
		}

		var user = await this.userRepository.GetById((int)userId);

		if (user == null)
		{
			return Unauthorized();
		}

		var rsos = await this.rsoRepository.GetAllByStudentId((int)userId);
		rsos.AddRange(await this.rsoRepository.GetAllByAdminId((int)userId));

		var rsoEventTasks = new List<Task<List<RSOEvent>>>();

		for (int i = 0; i < rsos.Count; i++)
		{
			rsoEventTasks.Add(this.eventRepository.GetAllRsoEvents(rsos[i].RSOID));
		}

		var rsoEvents = new List<RSOEvent>();

		foreach (var rsoEventList in await Task.WhenAll(rsoEventTasks))
		{
			rsoEvents.AddRange(rsoEventList);
		}

		var locationTasks = new List<Task<Location>>();

		for (int i = 0; i < rsoEvents.Count; i++)
		{
			locationTasks.Add(this.locationRepository.GetById(rsoEvents[i].LocID)!);
		}

		var response = this.mapper.Map<RsoEventResponse[]>(rsoEvents);

		var locations = await Task.WhenAll(locationTasks);

		for (int i = 0; i < locations.Length; i++)
		{
			response[i].Location = this.mapper.Map<LocationResponse>(locations[i]);
		}

		return Ok(response);
	}

	[Route("rso/byRso/{rsoId}")]
	[HttpGet]
	public async Task<IActionResult> GetAllRsoEvents([FromRoute] int rsoId)
	{
		int? userId = this.jwtTokenService.GetUserIdFromClaims(HttpContext.User.Claims.ToList());

		if (userId == null)
		{
			return Unauthorized();
		}

		var user = await this.userRepository.GetById((int)userId);

		if (user == null)
		{
			return Unauthorized();
		}

		var rsoEvents = await this.eventRepository.GetAllRsoEvents(rsoId);

		var locationTasks = new List<Task<Location>>();

		for (int i = 0; i < rsoEvents.Count; i++)
		{
			locationTasks.Add(this.locationRepository.GetById(rsoEvents[i].LocID)!);
		}

		var response = this.mapper.Map<RsoEventResponse[]>(rsoEvents);

		var locations = await Task.WhenAll(locationTasks);

		for (int i = 0; i < locations.Length; i++)
		{
			response[i].Location = this.mapper.Map<LocationResponse>(locations[i]);
		}

		return Ok(response);
	}
}