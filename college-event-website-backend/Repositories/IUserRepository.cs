using CollegeEvent.API.Models;

namespace CollegeEvent.API.Repositories;

public interface IUserRepository
{
	Task<User?> Create(User user);

	Task<User?> GetById(int id);

	Task<User?> GetByEmail(string email);

	Task<User?> Authenticate(string email, string password);

	Task<User?> AssignUniversity(int userId, int universityId);

	Task<User?> UpdateUserRole(int id, string newUserRole);
}
