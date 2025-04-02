using CollegeEvent.API.Dtos.Location;

namespace CollegeEvent.API.Dtos.Event;

public class UpdateEventRequest
{
	public string Name { get; set; } = null!;

	public string EventCategory { get; set; } = null!;

	public string? Description { get; set; }

	public DateTime EventDate { get; set; }

	public TimeSpan EventTime { get; set; }

	public TimeSpan EventTimeEnd { get; set; }

	public string? ContactPhone { get; set; }

	public string? ContactEmail { get; set; }

	public UpdateLocationRequest Location { get; set; } = null!;
}
