using System.ComponentModel.DataAnnotations;

namespace CollegeEvent.API.Dtos.Location;

public class AddLocationRequest
{

	[Required]
	[MinLength(1)]
	[MaxLength(100)]
	public string Name { get; set; } = null!;

	[Required]
	public double Latitude { get; set; }

	[Required]
	public double Longitude { get; set; }

	[Required]
	[MinLength(1)]
	[MaxLength(255)]
	public string Address { get; set; } = null!;
}