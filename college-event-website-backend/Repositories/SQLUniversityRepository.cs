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
			var numRowUpdated = await connection.ExecuteAsync("INSERT INTO universities (Name, Location, Description, NumStudents) VALUES (@Name, @Location, @Description, @NumStudents)", university, transaction);

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

		var university = await connection.QueryFirstOrDefaultAsync<University>("SELECT * FROM university WHERE UniversityID = @UniversityID", new { UniversityID = id });

		return university;
	}

	// Update university
	public async Task<University?> Update(int id, University university)
	{
		using var connection = GetConnection();

		try
		{
			// Update uni in DB
			var numRowUpdated = await connection.ExecuteAsync("UPDATE universities SET Name = @Name, Location = @Location, Description = @Description, NumStudents = @NumStudents WHERE UniversityID = @UniversityID", new
			{
				university.Name,
				university.Location,
				university.Description,
				university.NumStudents,
				UniversityID = id
			});

			if (numRowUpdated == 0)
			{
				return null;
			}

			university.UniversityID = id;
		}
		catch (Exception)
		{
			return null;
		}

		return university;
	}


	// Establish connection
	private MySqlConnection GetConnection()
	{
		return new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
	}
}