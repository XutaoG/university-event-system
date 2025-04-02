using System.ComponentModel.DataAnnotations;

namespace CollegeEvent.API.Dtos.Location;

public class LocationResponse
{
	public int LocID { get; set; }

	public string Name { get; set; } = null!;

	public double Latitude { get; set; }

	public double Longitude { get; set; }

	public string Address { get; set; } = null!;
}