namespace CollegeEvent.API.Dtos.Event;

public class PublicEventResponse : EventResponse
{
	public int UniversityID { get; set; }

	public bool Approved { get; set; }
}