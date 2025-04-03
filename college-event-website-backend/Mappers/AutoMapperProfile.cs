using AutoMapper;
using CollegeEvent.API.Dto.Comment;
using CollegeEvent.API.Dtos.Auth;
using CollegeEvent.API.Dtos.Comment;
using CollegeEvent.API.Dtos.Event;
using CollegeEvent.API.Dtos.Location;
using CollegeEvent.API.Dtos.RSO;
using CollegeEvent.API.Dtos.University;
using CollegeEvent.API.Models;

namespace CollegeEvent.API.Mappers;

public class AutoMapperProfile : Profile
{
	public AutoMapperProfile()
	{
		// Auth
		CreateMap<User, UserResponse>();

		// University
		CreateMap<AddUniversityRequest, University>();
		CreateMap<UpdateUniversityRequest, University>();
		CreateMap<University, UniversityResponse>();

		// RSO
		CreateMap<AddRSORequest, RSO>();
		CreateMap<UpdateRSORequest, RSO>();
		CreateMap<RSO, RSOResponse>();

		// Location
		CreateMap<AddLocationRequest, Location>();
		CreateMap<UpdateLocationRequest, Location>();
		CreateMap<Location, LocationResponse>();

		// Public Event
		CreateMap<AddPublicEventRequest, PublicEvent>();
		CreateMap<PublicEvent, PublicEventResponse>();

		// Private Event
		CreateMap<AddPrivateEventRequest, PrivateEvent>();
		CreateMap<PrivateEvent, PrivateEventResponse>();

		// RSO Event
		CreateMap<AddRsoEventRequest, RSOEvent>();
		CreateMap<RSOEvent, RsoEventResponse>();

		// Event
		CreateMap<UpdateEventRequest, Event>();
		CreateMap<Event, EventResponse>();

		// Comment
		CreateMap<AddCommentRequest, Comment>();
		CreateMap<UpdateCommentRequest, Comment>();
		CreateMap<Comment, CommentResponse>();
	}
}