using CollegeEvent.API.Models;

namespace CollegeEvent.API.Repositories;

public interface IUniversityRepository
{
	Task<University?> Create(University university);

	Task<University?> GetById(int id);

	Task<University?> Update(int id, University university);

	// Task<University?> GetUniversityByUserId(int id);
}
