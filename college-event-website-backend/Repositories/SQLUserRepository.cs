using CollegeEvent.API.Models;
using CollegeEvent.API.Services;
using Dapper;
using MySql.Data.MySqlClient;
using Mysqlx;

namespace CollegeEvent.API.Repositories;

public class SQLUserRepository : IUserRepository
{
	private readonly IConfiguration configuration;
	private readonly PasswordHashService passwordHashService;

	public SQLUserRepository(IConfiguration configuration, PasswordHashService passwordHashService)
	{
		this.configuration = configuration;
		this.passwordHashService = passwordHashService;
	}

	// Create user
	public async Task<User?> Create(User user)
	{
		using var connection = GetConnection();

		try
		{
			// Insert user into DB
			var numRowUpdated = await connection.ExecuteAsync("INSERT INTO users (Name, Email, PasswordHash, UserRole) VALUES (@Name, @Email, @PasswordHash, @UserRole)", user);

			if (numRowUpdated == 0)
			{
				return null;
			}
		}
		catch (Exception)
		{
			return null;
		}

		return user;
	}

	// Get user
	public async Task<User?> GetById(int id)
	{
		using var connection = GetConnection();

		// Find user with UID
		var user = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM users WHERE UID = @UID", new { UID = id });

		return user;
	}

	// Authenticate user 
	public async Task<User?> Authenticate(string email, string password)
	{
		using var connection = GetConnection();

		// Find user with email
		var foundUser = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM users WHERE Email = @Email", new { Email = email });

		// Check for existence
		if (foundUser == null)
		{
			return null;
		}

		// Check if password matches
		if (this.passwordHashService.VerifyPassword(foundUser.PasswordHash, password))
		{
			return foundUser;
		}

		return null;
	}

	public async Task<User?> AssignUniversity(int userId, int universityId)
	{
		using var connection = GetConnection();

		try
		{
			// Find user with email
			var numRowUpdated = await connection.ExecuteAsync("UPDATE users SET UniversityID = @UniversityID WHERE UID = @UID", new { UniversityID = universityId, UID = userId });

			if (numRowUpdated == 0)
			{
				return null;
			}
		}
		catch (Exception)
		{
			return null;
		}

		return await GetById(userId);
	}

	// Establish connection
	private MySqlConnection GetConnection()
	{
		return new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
	}
}
