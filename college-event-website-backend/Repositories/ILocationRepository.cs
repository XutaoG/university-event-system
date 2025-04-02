using CollegeEvent.API.Models;

namespace CollegeEvent.API.Repositories;

public interface ILocationRepository
{
	Task<Location?> Create(Location location);

	Task<Location?> GetById(int id);

	Task<Location?> Update(int id, Location location);

	Task<Location?> Delete(int id);
}