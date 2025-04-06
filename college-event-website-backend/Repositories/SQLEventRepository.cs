using AutoMapper;
using CollegeEvent.API.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace CollegeEvent.API.Repositories;

public class SQLEventRepository(
	IConfiguration configuration) : IEventRepository
{
	private readonly IConfiguration configuration = configuration;

	public async Task<PublicEvent?> CreatePublicEvent(PublicEvent publicEvent)
	{
		using var connection = GetConnection();

		try
		{
			await connection.OpenAsync();
			using var transaction = await connection.BeginTransactionAsync();

			// Insert event into DB
			var numRowUpdated = await connection.ExecuteAsync(@"INSERT INTO events (Name, EventCategory, Description, EventDate, EventTime, EventTimeEnd, ContactPhone, ContactEmail, LocID, AdminID) 
								VALUES (@Name, @EventCategory, @Description, @EventDate, @EventTime, @EventTimeEnd, @ContactPhone, @ContactEmail, @LocID, @AdminID)", publicEvent);

			if (numRowUpdated == 0)
			{
				return null;
			}

			// Get last inserted ID
			var insertedId = await connection.QuerySingleAsync<int>("SELECT LAST_INSERT_ID()", transaction: transaction);

			publicEvent.EventID = insertedId;

			// Insert public event into DB
			numRowUpdated = await connection.ExecuteAsync(@"INSERT INTO public_events (EventID, UniversityID, Approved) 
															VALUES (@EventID, @UniversityID, @Approved)", publicEvent);

			if (numRowUpdated == 0)
			{
				return null;
			}

			await transaction.CommitAsync();

			return await GetPublicEventById(insertedId);
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async Task<PrivateEvent?> CreatePrivateEvent(PrivateEvent privateEvent)
	{
		using var connection = GetConnection();

		try
		{
			await connection.OpenAsync();
			using var transaction = await connection.BeginTransactionAsync();

			// Insert event into DB
			var numRowUpdated = await connection.ExecuteAsync(@"INSERT INTO events (Name, EventCategory, Description, EventDate, EventTime, EventTimeEnd, ContactPhone, ContactEmail, LocID, AdminID) 
								VALUE (@Name, @EventCategory, @Description, @EventDate, @EventTime, @EventTimeEnd, @ContactPhone, @ContactEmail, @LocID, @AdminID)", privateEvent);

			if (numRowUpdated == 0)
			{
				return null;
			}

			// Get last inserted ID
			var insertedId = await connection.QuerySingleAsync<int>("SELECT LAST_INSERT_ID()", transaction: transaction);

			privateEvent.EventID = insertedId;

			// Insert private event into DB
			numRowUpdated = await connection.ExecuteAsync(@"INSERT INTO private_events (EventID, UniversityID) 
															VALUE (@EventID, @UniversityID)", privateEvent);

			if (numRowUpdated == 0)
			{
				return null;
			}

			await transaction.CommitAsync();

			return await GetPrivateEventById(insertedId);
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async Task<RSOEvent?> CreateRsoEvent(RSOEvent rsoEvent)
	{
		using var connection = GetConnection();

		try
		{
			await connection.OpenAsync();
			using var transaction = await connection.BeginTransactionAsync();

			// Insert event into DB
			var numRowUpdated = await connection.ExecuteAsync(@"INSERT INTO events (Name, EventCategory, Description, EventDate, EventTime, EventTimeEnd, ContactPhone, ContactEmail, LocID, AdminID) 
								VALUE (@Name, @EventCategory, @Description, @EventDate, @EventTime, @EventTimeEnd, @ContactPhone, @ContactEmail, @LocID, @AdminID)", rsoEvent);

			if (numRowUpdated == 0)
			{
				return null;
			}

			// Get last inserted ID
			var insertedId = await connection.QuerySingleAsync<int>("SELECT LAST_INSERT_ID()", transaction: transaction);
			rsoEvent.EventID = insertedId;

			// Insert RSO event into DB
			numRowUpdated = await connection.ExecuteAsync(@"INSERT INTO rso_events (EventID, RSOID) 
															VALUE (@EventID, @RSOID)", rsoEvent);

			if (numRowUpdated == 0)
			{
				return null;
			}

			await transaction.CommitAsync();

			return await GetRsoEventById(insertedId);
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async Task<PublicEvent?> GetPublicEventById(int id)
	{
		using var connection = GetConnection();

		try
		{
			// Get public event from DB
			var publicEvent = await connection.QueryFirstOrDefaultAsync<PublicEvent>(@"SELECT e.*, p.UniversityID, p.Approved FROM events e
																					INNER JOIN public_events p ON e.EventID = p.EventID WHERE e.EventID = @EventID;", new { EventID = id });

			if (publicEvent == null)
			{
				return null;
			}

			return publicEvent;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async Task<PrivateEvent?> GetPrivateEventById(int id)
	{
		using var connection = GetConnection();

		try
		{
			// Get private event from DB
			var privateEvent = await connection.QueryFirstOrDefaultAsync<PrivateEvent>(@"SELECT e.*, p.UniversityID FROM events e
																						INNER JOIN private_events p ON e.EventID = p.EventID WHERE e.EventID = @EventID;", new { EventID = id });

			if (privateEvent == null)
			{
				return null;
			}

			return privateEvent;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async Task<RSOEvent?> GetRsoEventById(int id)
	{
		using var connection = GetConnection();

		try
		{
			// Get RSO event from DB
			var rsoEvent = await connection.QueryFirstOrDefaultAsync<RSOEvent>(@"SELECT e.*, p.RSOID FROM events e
																				INNER JOIN rso_events p ON e.EventID = p.EventID WHERE e.EventID = @EventID;", new { EventID = id });

			if (rsoEvent == null)
			{
				return null;
			}

			return rsoEvent;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async Task<Event?> GetEventById(int id)
	{
		using var connection = GetConnection();

		try
		{
			// Get public event from DB
			var event_ = await connection.QueryFirstOrDefaultAsync<Event>(@"SELECT * FROM events WHERE EventID = @EventID;", new { EventID = id });

			if (event_ == null)
			{
				return null;
			}

			return event_;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async Task<Event?> UpdateEvent(int id, Event event_)
	{
		using var connection = GetConnection();

		try
		{
			await connection.OpenAsync();
			using var transaction = await connection.BeginTransactionAsync();

			// Check if ID exists
			var foundEvent = await connection.QueryFirstOrDefaultAsync<Event>(@"SELECT * FROM events WHERE EventID = @EventID;", new { EventID = id });

			if (foundEvent == null)
			{
				return null;
			}

			foundEvent.Name = event_.Name;
			foundEvent.EventCategory = event_.EventCategory;
			foundEvent.Description = event_.Description;
			foundEvent.EventTime = event_.EventTime;
			foundEvent.ContactPhone = event_.ContactPhone;
			foundEvent.ContactEmail = event_.ContactEmail;

			// Update event
			var numRowUpdated = await connection.ExecuteAsync(@"UPDATE events SET Name = @Name, EventCategory = @EventCategory, Description = @Description, 
																EventTime = @EventTime, ContactPhone = @ContactPhone, ContactEmail = @ContactEmail WHERE EventId = @EventId", foundEvent);

			if (numRowUpdated == 0)
			{
				return null;
			}

			await transaction.CommitAsync();

			return foundEvent;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async Task<Event?> DeleteEvent(int id)
	{
		using var connection = GetConnection();

		try
		{
			await connection.OpenAsync();
			using var transaction = await connection.BeginTransactionAsync();

			// Check if ID exists
			var foundEvent = await connection.QueryFirstOrDefaultAsync<Event>("SELECT * FROM events WHERE EventID = @EventID", new { EventID = id });

			if (foundEvent == null)
			{
				return null;
			}

			// Delete event from DB
			int numRowUpdated = await connection.ExecuteAsync("DELETE FROM events WHERE EventID = @EventID", new { EventID = id });

			if (numRowUpdated == 0)
			{
				return null;
			}

			await transaction.CommitAsync();

			return foundEvent;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async Task<PublicEvent?> SetPublicEventApproved(int id)
	{
		using var connection = GetConnection();

		try
		{
			await connection.OpenAsync();
			using var transaction = await connection.BeginTransactionAsync();

			// Check if ID exists
			var foundEvent = await connection.QueryFirstOrDefaultAsync<PublicEvent>("SELECT * FROM public_events WHERE EventID = @EventID", new { EventID = id });

			if (foundEvent == null)
			{
				return null;
			}

			// Update approved in DB
			int numRowUpdated = await connection.ExecuteAsync("UPDATE public_events SET Approved = true WHERE EventID = @EventID", new { EventID = id });

			if (numRowUpdated == 0)
			{
				return null;
			}

			await transaction.CommitAsync();

			return await GetPublicEventById(id);
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async Task<List<PublicEvent>> GetAllPublicEvents()
	{
		using var connection = GetConnection();

		try
		{
			var pubilcEvents = await connection.QueryAsync<PublicEvent>(@"SELECT e.*, p.Approved, p.UniversityID FROM events e
																		INNER JOIN public_events p ON e.EventID = p.EventID");

			return pubilcEvents.ToList();
		}
		catch (Exception)
		{
			return [];
		}
	}

	public async Task<List<PrivateEvent>> GetAllPrivateEvents(int universityId)
	{
		using var connection = GetConnection();

		try
		{
			var privateEvents = await connection.QueryAsync<PrivateEvent>(@"SELECT e.*, p.UniversityID FROM events e
																		INNER JOIN private_events p ON e.EventID = p.EventID WHERE p.UniversityID = @UniversityID",
																		new { UniversityID = universityId });

			return privateEvents.ToList();
		}
		catch (Exception)
		{
			return [];
		}
	}

	public async Task<List<RSOEvent>> GetAllRsoEvents(int rsoId)
	{
		using var connection = GetConnection();

		try
		{
			var rsoEvents = await connection.QueryAsync<RSOEvent>(@"SELECT e.*, p.RSOID FROM events e
																		INNER JOIN rso_events p ON e.EventID = p.EventID WHERE p.RSOID = @RSOID",
																		new { RSOID = rsoId });

			return rsoEvents.ToList();
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