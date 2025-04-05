using System.ComponentModel.Design;
using CollegeEvent.API.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace CollegeEvent.API.Repositories;

public class SQLUniversityRepository(IConfiguration configuration) : IUniversityRepository
{
	private readonly IConfiguration configuration = configuration;

	// Create university
	public async Task<University?> Create(University university)
	{
		using var connection = GetConnection();

		try
		{
			await connection.OpenAsync();
			using var transaction = await connection.BeginTransactionAsync();

			// Insert uni into DB
			var numRowUpdated = await connection.ExecuteAsync("INSERT INTO universities (Name, Location, Description, NumStudents, Domain) VALUES (@Name, @Location, @Description, @NumStudents, @Domain)", university, transaction);

			if (numRowUpdated == 0)
			{
				return null;
			}

			// Get last inserted ID
			var insertedId = await connection.QuerySingleAsync<int>("SELECT LAST_INSERT_ID()", transaction: transaction);

			await transaction.CommitAsync();

			university.UniversityID = insertedId;
		}
		catch (Exception)
		{
			return null;
		}

		return university;
	}

	// Get uni by ID
	public async Task<University?> GetById(int id)
	{
		using var connection = GetConnection();

		try
		{
			var university = await connection.QueryFirstOrDefaultAsync<University>("SELECT * FROM universities WHERE UniversityID = @UniversityID", new { UniversityID = id });
			return university;
		}
		catch (Exception)
		{
			return null;
		}
	}

	// Update university
	public async Task<University?> Update(int id, University university)
	{
		using var connection = GetConnection();

		try
		{
			await connection.OpenAsync();
			using var transaction = await connection.BeginTransactionAsync();

			// Check if ID exists
			var foundUniversity = await connection.QueryFirstOrDefaultAsync<University>("SELECT * FROM universities WHERE UniversityID = @UniversityID", new { UniversityID = id });

			if (foundUniversity == null)
			{
				return null;
			}

			foundUniversity.Name = university.Name;
			foundUniversity.Location = university.Location;
			foundUniversity.Description = university.Description;
			foundUniversity.NumStudents = university.NumStudents;

			// Update uni in DB
			var numRowUpdated = await connection.ExecuteAsync("UPDATE universities SET Name = @Name, Location = @Location, Description = @Description, NumStudents = @NumStudents WHERE UniversityID = @UniversityID", foundUniversity);

			if (numRowUpdated == 0)
			{
				return null;
			}

			await transaction.CommitAsync();

			return foundUniversity;
		}
		catch (Exception)
		{
			return null;
		}

	}

	public async Task<University?> GetUniversityByDomain(string domain)
	{
		using var connection = GetConnection();

		try
		{
			var foundUniversity = await connection.QueryFirstOrDefaultAsync<University>("SELECT * FROM universities WHERE Domain = @Domain", new { Domain = domain });

			return foundUniversity;
		}
		catch (Exception)
		{
			return null;
		}
	}

	// public async Task<University?> GetUniversityByUserId(int id)
	// {
	// 	using var connection = GetConnection();

	// 	try
	// 	{
	// 		await connection.OpenAsync();
	// 		using var transaction = await connection.BeginTransactionAsync();

	// 		var foundUser = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM users WHERE UID = @UID", new { UID = id });

	// 		if (foundUser == null)
	// 		{
	// 			return null;
	// 		}

	// 		var foundUniversity = await connection.QueryFirstOrDefaultAsync<University>("SELECT * FROM universities WHERE Domain = @Domain", new { Domain = foundUser.Email.Split("@").Last() });

	// 		await transaction.CommitAsync();

	// 		return foundUniversity;
	// 	}
	// 	catch (Exception)
	// 	{
	// 		return null;
	// 	}
	// }


	// Establish connection
	private MySqlConnection GetConnection()
	{
		return new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
	}
}