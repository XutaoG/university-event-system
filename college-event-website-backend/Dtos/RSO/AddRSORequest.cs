using System.ComponentModel.DataAnnotations;

namespace CollegeEvent.API.Dtos.RSO;


public class AddRSORequest
{
	[Required]
	[MinLength(2)]
	[MaxLength(100)]
	public string Name { get; set; } = null!;

	[Required]
	public string Description { get; set; } = null!;

	public List<string> MemberEmails { get; set; } = [];
}
