namespace CollegeEvent.API.Models
{
	public class Comment
	{
		public int CommentID { get; set; }

		public int UID { get; set; }

		public int EventID { get; set; }

		public string Text { get; set; } = null!;

		public int Rating { get; set; }

		public DateTime Timestamp { get; set; }

	}
}