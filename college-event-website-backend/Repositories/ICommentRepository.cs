using CollegeEvent.API.Models;

namespace CollegeEvent.API.Repositories;

public interface ICommentRepository
{
	Task<Comment?> Add(Comment comment);

	Task<Comment?> GetById(int id);

	Task<Comment?> UpdateById(int id, Comment comment);

	Task<Comment?> Delete(int id);

	Task<List<Comment>> GetAllByEventId(int eventId);
}