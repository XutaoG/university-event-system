namespace CollegeEvent.API.Dtos.Comment;

public class AddCommentRequest
{
	public string Text { get; set; } = null!;

	public int Rating { get; set; }
}