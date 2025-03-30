using CollegeEvent.API.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace CollegeEvent.API.Repositories;

public class SQLRSORepository(
	IConfiguration configuration,
	IUniversityRepository universityRepository
	) : IRSORepository
{
	private readonly IConfiguration configuration = configuration;
	private readonly IUniversityRepository universityRepository = universityRepository;

	// Create RSO
	public async Task<RSO?> Create(int adminId, RSO rso)
	{
		using var connection = GetConnection();

		try
		{
			await connection.OpenAsync();
			using var transaction = await connection.BeginTransactionAsync();

			// Get UniversityID
			var user = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM users WHERE UID = @UID", new { UID = adminId });
			if (user == null || user.UniversityID == null)
			{
				return null;
			}

			rso.UniversityID = (int)user.UniversityID;

			// Insert RSO into DB
			rso.AdminID = adminId;
			var numRowUpdated = await connection.ExecuteAsync("INSERT INTO RSOs (Name, UniversityID, AdminID, Active) VALUES (@Name, @UniversityID, @AdminID, false)", rso);

			if (numRowUpdated == 0)
			{
				return null;
			}

			// Get last inserted ID
			var insertedId = await connection.QuerySingleAsync<int>("SELECT LAST_INSERT_ID()", transaction: transaction);

			await transaction.CommitAsync();

			rso.RSOID = insertedId;

			return rso;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async Task<RSO?> GetById(int id)
	{
		using var connection = GetConnection();

		try
		{
			var rso = await connection.QueryFirstOrDefaultAsync<RSO>("SELECT * FROM rsos WHERE RSOID = @RSOID", new { RSOID = id });
			return rso;
		}
		catch (Exception)
		{
			return null;
		}

	}

	public async Task<RSO?> Update(int id, RSO rso)
	{
		using var connection = GetConnection();

		try
		{
			await connection.OpenAsync();
			using var transaction = await connection.BeginTransactionAsync();

			// Check if ID exists
			var foundRso = await connection.QueryFirstOrDefaultAsync<RSO>("SELECT * FROM rsos WHERE RSOID = @RSOID", new { RSOID = id });

			if (foundRso == null)
			{
				return null;
			}

			foundRso.Name = rso.Name;

			// Update RSO
			var numRowUpdated = await connection.ExecuteAsync("UPDATE rsos SET Name = @Name WHERE RSOID = @RSOID", new { foundRso.Name, RSOID = id });

			if (numRowUpdated == 0)
			{
				return null;
			}

			await transaction.CommitAsync();

			return foundRso;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async Task<RSO?> Delete(int id)
	{
		using var connection = GetConnection();

		try
		{
			await connection.OpenAsync();
			using var transaction = await connection.BeginTransactionAsync();

			// Check if ID exists
			var foundRso = await connection.QueryFirstOrDefaultAsync<RSO>("SELECT * FROM rsos WHERE RSOID = @RSOID", new { RSOID = id });

			if (foundRso == null)
			{
				return null;
			}

			// Delete RSO from DB
			int numRowUpdated = await connection
				.ExecuteAsync("DELETE FROM rsos WHERE RSOID = @RSOID", new { RSOID = id });

			if (numRowUpdated == 0)
			{
				return null;
			}

			await transaction.CommitAsync();

			return foundRso;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async Task<List<RSO>> GetAllByAdminID(int adminID)
	{
		using var connection = GetConnection();

		try
		{
			var rsos = await connection.QueryAsync<RSO>("SELECT * FROM rsos WHERE AdminID = @AdminID", new { AdminID = adminID });
			return rsos.ToList();
		}
		catch (Exception)
		{
			return [];
		}
	}

	// Establish connection
	private MySqlConnection GetConnection()
	{
		return new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
	}
}