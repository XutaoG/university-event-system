using CollegeEvent.API.Models;

namespace CollegeEvent.API.Repositories;

public interface IRSORepository
{
	Task<RSO?> Create(int adminID, RSO rso, List<string> emails);

	Task<RSO?> GetById(int id);

	Task<RSO?> Update(int id, RSO rso);

	Task<RSO?> Delete(int id);

	Task<List<RSO>> GetAllByAdminId(int adminId);

	Task<List<RSO>> GetAllByStudentId(int studentId);

	Task<List<RSO>> GetAllByAvailability(int id);

	Task<bool> CreateRsoMembers(int uid, int rsoId);

	Task<bool> DeleteRsoMembers(int uid, int rsoId);
}