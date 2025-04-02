using CollegeEvent.API.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace CollegeEvent.API.Repositories;

public class SQLLocationRepository(IConfiguration configuration) : ILocationRepository
{
	private readonly IConfiguration configuration = configuration;

	public async Task<Location?> Create(Location location)
	{
		using var connection = GetConnection();

		try
		{
			await connection.OpenAsync();
			using var transaction = await connection.BeginTransactionAsync();

			// Insert location into DB
			var numRowUpdated = await connection.ExecuteAsync("INSERT INTO locations (Name, Latitude, Longitude, Address) VALUES (@Name, @Latitude, @Longitude, @Address)", location);

			if (numRowUpdated == 0)
			{
				return null;
			}

			// Get last inserted ID
			var insertedId = await connection.QuerySingleAsync<int>("SELECT LAST_INSERT_ID()", transaction: transaction);

			await transaction.CommitAsync();

			location.LocID = insertedId;

			return location;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async Task<Location?> GetById(int id)
	{
		using var connection = GetConnection();

		try
		{
			var location = await connection.QueryFirstOrDefaultAsync<Location>("SELECT * FROM locations WHERE LocID = @LocID", new { LocID = id });
			return location;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async Task<Location?> Update(int id, Location location)
	{
		using var connection = GetConnection();

		try
		{
			await connection.OpenAsync();
			using var transaction = await connection.BeginTransactionAsync();

			// Check if ID exists
			var foundLocation = await connection.QueryFirstOrDefaultAsync<Location>("SELECT * FROM locations WHERE LocID = @LocID", new { LocID = id });

			if (foundLocation == null)
			{
				return null;
			}

			foundLocation.Name = location.Name;
			foundLocation.Latitude = location.Latitude;
			foundLocation.Longitude = location.Longitude;
			foundLocation.Address = location.Address;

			// Update location
			var numRowUpdated = await connection.ExecuteAsync("UPDATE location SET Name = @Name, Latitude = @Latitude, Longitude = @Longitude, Addres = @Address WHERE LocID = @LocID", foundLocation);

			if (numRowUpdated == 0)
			{
				return null;
			}

			await transaction.CommitAsync();

			return foundLocation;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async Task<Location?> Delete(int id)
	{
		using var connection = GetConnection();

		try
		{
			await connection.OpenAsync();
			using var transaction = await connection.BeginTransactionAsync();

			// Check if ID exists
			var foundLocation = await connection.QueryFirstOrDefaultAsync<Location>("SELECT * FROM locations WHERE LocID = @LocID", new { LocID = id });

			if (foundLocation == null)
			{
				return null;
			}

			// Delete location from DB
			int numRowUpdated = await connection.ExecuteAsync("DELETE FROM locations WHERE LocID = @LocID", new { LocID = id });

			if (numRowUpdated == 0)
			{
				return null;
			}

			await transaction.CommitAsync();

			return foundLocation;
		}
		catch (Exception)
		{
			return null;
		}
	}

	// Establish connection
	private MySqlConnection GetConnection()
	{
		return new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
	}
}