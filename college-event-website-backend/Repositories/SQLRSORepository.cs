using CollegeEvent.API.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace CollegeEvent.API.Repositories;

public class SQLRSORepository(
	IConfiguration configuration
	) : IRSORepository
{
	private readonly IConfiguration configuration = configuration;

	// Create RSO
	public async Task<RSO?> Create(int adminId, RSO rso, List<string> memberEmails)
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

			rso.RSOID = insertedId;

			// Verify email existence
			var emailVerifyTasks = new List<Task<User?>>();

			foreach (var email in memberEmails)
			{
				emailVerifyTasks.Add(connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM users WHERE Email = @Email", new { Email = email }));
			}

			foreach (var res in await Task.WhenAll(emailVerifyTasks))
			{
				if (res == null)
				{
					return null;
				}
			}

			// Add members
			var memberInsertTasks = new List<Task<int>>();

			foreach (var email in memberEmails)
			{
				memberInsertTasks.Add(connection.ExecuteAsync("INSERT INTO rso_members (RSOID, UID) VALUES (@RSOID, (SELECT UID FROM users WHERE EMAIL = @EMAIL))", new { RSOID = insertedId, EMAIL = email }));
			}

			foreach (var res in await Task.WhenAll(memberInsertTasks))
			{
				if (res == 0)
				{
					return null;
				}
			}

			await transaction.CommitAsync();

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

	public async Task<List<RSO>> GetAllByAdminId(int adminId)
	{
		using var connection = GetConnection();

		try
		{
			var rsos = await connection.QueryAsync<RSO>("SELECT * FROM rsos WHERE AdminID = @AdminID", new { AdminID = adminId });
			return rsos.ToList();
		}
		catch (Exception)
		{
			return [];
		}
	}

	public async Task<List<RSO>> GetAllByStudentId(int studentId)
	{
		using var connection = GetConnection();

		try
		{
			var rsos = await connection.QueryAsync<RSO>("SELECT * FROM rsos WHERE RSOID IN (SELECT RSOID FROM rso_members WHERE UID = @UID);", new { UID = studentId });
			return rsos.ToList();
		}
		catch (Exception)
		{
			return [];
		}
	}

	public async Task<bool> CreateRsoMembers(int uid, int rsoId)
	{
		using var connection = GetConnection();

		try
		{
			// Insert RSO_Members into DB
			var numRowUpdated = await connection.ExecuteAsync("INSERT INTO rso_members (RSOID, UID) VALUES (@RSOID, @UID)", new { RSOID = rsoId, UID = uid });
			if (numRowUpdated == 0)
			{
				return false;
			}

			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public async Task<bool> DeleteRsoMembers(int uid, int rsoId)
	{
		using var connection = GetConnection();

		try
		{
			// delete RSO_Members into DB
			var numRowUpdated = await connection.ExecuteAsync("DELETE FROM rso_members WHERE RSOID = @RSOID AND UID = @UID", new { RSOID = rsoId, UID = uid });
			if (numRowUpdated == 0)
			{
				return false;
			}

			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public async Task<List<RSO>> GetAllByAvailability(int id)
	{
		using var connection = GetConnection();

		try
		{
			var rsos = await connection.QueryAsync<RSO>("SELECT * FROM rsos WHERE RSOID NOT IN (SELECT RSOID FROM rso_members WHERE UID = @UID)", new { UID = id });
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