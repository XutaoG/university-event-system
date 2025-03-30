using CollegeEvent.API.Models;
using Microsoft.OpenApi.Models;

namespace CollegeEvent.API.Repositories;

public interface IRSORepository
{
	Task<RSO?> Create(int adminID, RSO rso);

	Task<RSO?> GetById(int id);

	Task<RSO?> Update(int id, RSO rso);

	Task<RSO?> Delete(int id);

	Task<List<RSO>> GetAllByAdminID(int adminID);
}