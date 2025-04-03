namespace CollegeEvent.API.Dtos.Comment;

public class UpdateCommentRequest
{
	public string Text { get; set; } = null!;

	public int Rating { get; set; }
}