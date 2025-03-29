using AutoMapper;
using CollegeEvent.API.Dtos.Auth;
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
	}
}