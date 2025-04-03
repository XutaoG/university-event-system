using CollegeEvent.API.Dtos.Auth;

namespace CollegeEvent.API.Dto.Comment;

public class CommentResponse
{
	public int CommentID { get; set; }

	public int UID { get; set; }

	public UserResponse UserResponse { get; set; } = null!;

	public int EventID { get; set; }

	public string Text { get; set; } = null!;

	public int Rating { get; set; }

	public DateTime Timestamp { get; set; }
}
