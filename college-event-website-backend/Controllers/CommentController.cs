using AutoMapper;
using CollegeEvent.API.Dto.Comment;
using CollegeEvent.API.Dtos.Auth;
using CollegeEvent.API.Dtos.Comment;
using CollegeEvent.API.Models;
using CollegeEvent.API.Repositories;
using CollegeEvent.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollegeEvent.API.Controller;

[ApiController]
[Route("api/comment")]
[Authorize]
public class CommentController(
	IMapper mapper,
	JwtTokenService jwtTokenService,
	ICommentRepository commentRepository,
	IEventRepository eventRepository,
	IUserRepository userRepository) : ControllerBase
{
	private readonly IMapper mapper = mapper;
	private readonly JwtTokenService jwtTokenService = jwtTokenService;
	private readonly ICommentRepository commentRepository = commentRepository;
	private readonly IEventRepository eventRepository = eventRepository;
	private readonly IUserRepository userRepository = userRepository;

	[HttpPost]
	[Route("{eventId}")]
	public async Task<IActionResult> AddComment([FromRoute] int eventId, [FromBody] AddCommentRequest addCommentRequest)
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

		// Check event ID
		var event_ = await this.eventRepository.GetEventById(eventId);

		if (event_ == null)
		{
			return NotFound();
		}

		// Check user
		var user = await this.userRepository.GetById((int)userId);

		if (user == null)
		{
			return Unauthorized();
		}

		// Add comment to DB
		var comment = this.mapper.Map<Comment>(addCommentRequest);
		comment.UID = user.UID;
		comment.EventID = event_.EventID;

		comment = await this.commentRepository.Add(comment);

		if (comment == null)
		{
			return BadRequest();
		}

		var userResponse = this.mapper.Map<UserResponse>(user);
		var response = this.mapper.Map<CommentResponse>(comment);
		response.UserResponse = userResponse;

		return CreatedAtAction(nameof(GetComment), new { id = response.CommentID }, response);
	}

	[HttpGet]
	[Route("{id}")]
	public async Task<IActionResult> GetComment([FromRoute] int id)
	{
		// Get comment
		var foundComment = await this.commentRepository.GetById(id);

		if (foundComment == null)
		{
			return NotFound();
		}

		// Get user
		var foundUser = await this.userRepository.GetById(foundComment.UID);

		if (foundUser == null)
		{
			return NotFound();
		}

		var userResponse = this.mapper.Map<UserResponse>(foundUser);
		var response = this.mapper.Map<CommentResponse>(foundComment);
		response.UserResponse = userResponse;

		return Ok(response);
	}

	[HttpPut]
	[Route("{id}")]
	public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentRequest updateCommentRequest)
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

		var foundComment = await this.commentRepository.GetById(id);

		if (foundComment == null)
		{
			return NotFound();
		}

		// Check ifuser has permission
		if (foundComment.UID != userId)
		{
			return Unauthorized();
		}

		var comment = this.mapper.Map<Comment>(updateCommentRequest);

		comment = await this.commentRepository.UpdateById(id, comment);

		if (comment == null)
		{
			return BadRequest();
		}

		// Get user
		var foundUser = await this.userRepository.GetById(comment.UID);

		if (foundUser == null)
		{
			return NotFound();
		}

		var userResponse = this.mapper.Map<UserResponse>(foundUser);
		var response = this.mapper.Map<CommentResponse>(comment);
		response.UserResponse = userResponse;

		return Ok(response);
	}

	[HttpDelete]
	[Route("{id}")]
	public async Task<IActionResult> DeleteComment([FromRoute] int id)
	{
		int? userId = this.jwtTokenService.GetUserIdFromClaims(HttpContext.User.Claims.ToList());

		if (userId == null)
		{
			return Unauthorized();
		}

		var comment = await this.commentRepository.GetById(id);

		if (comment == null)
		{
			return NotFound();
		}

		// Check user permission
		if (comment.UID != (int)userId)
		{
			return Unauthorized();
		}

		await this.commentRepository.Delete(id);

		return NoContent();
	}

	[HttpGet]
	[Route("event/{eventId}")]
	public async Task<IActionResult> GetAllCommentFromEvent([FromRoute] int eventId)
	{
		var event_ = await this.eventRepository.GetEventById(eventId);

		if (event_ == null)
		{
			return NotFound();
		}

		var comments = await this.commentRepository.GetAllByEventId(eventId);

		var response = this.mapper.Map<List<CommentResponse>>(comments);

		// Attach user responses
		var userTasks = new List<Task<User>>();

		foreach (var commentResponse in response)
		{
			if (this.userRepository.GetById(commentResponse.UID) == null)
			{
				return NotFound();
			}

			userTasks.Add(this.userRepository.GetById(commentResponse.UID)!);
		}

		var users = await Task.WhenAll(userTasks);

		for (int i = 0; i < response.Count; i++)
		{
			response[i].UserResponse = this.mapper.Map<UserResponse>(users[i]);
		}

		return Ok(response);
	}
}