using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CollegeEvent.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace CollegeEvent.API.Services;

public class JwtTokenService
{
	private readonly IConfiguration configuration;

	public JwtTokenService(IConfiguration configuration)
	{
		this.configuration = configuration;
	}

	public string Create(User user)
	{
		byte[] secretKey = Encoding.ASCII.GetBytes(this.configuration.GetValue<string>("Jwt:secret_key") ?? "");
		DateTime expiryTime = DateTime.UtcNow.AddMinutes(30);

		var tokenHandler = new JwtSecurityTokenHandler();

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(
				[
					new Claim("UID", user.UID.ToString()),
					new Claim("Email", user.Email),
					new Claim("userRole", user.UserRole)
				]
			),
			NotBefore = DateTime.UtcNow,
			Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256),
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);

		return tokenHandler.WriteToken(token);
	}

	public int? GetUserIdFromClaims(List<Claim> claims)
	{
		Claim? userIdClaim = claims.FirstOrDefault(c => c.Type == "UID");

		if (userIdClaim == null)
		{
			return null;
		}

		if (int.TryParse(userIdClaim.Value, out int claimUserId))
		{
			return claimUserId;
		}

		return null;
	}
}