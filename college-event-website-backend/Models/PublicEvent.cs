namespace CollegeEvent.API.Models;

public class PublicEvent : Event
{
	public int UniversityID { get; set; }

	public bool Approved { get; set; }
}
