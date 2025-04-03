using CollegeEvent.API.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace CollegeEvent.API.Repositories;

public class SQLCommentRepository(IConfiguration configuration) : ICommentRepository
{
	private readonly IConfiguration configuration = configuration;

	public async Task<Comment?> Add(Comment comment)
	{
		using var connection = GetConnection();

		try
		{
			await connection.OpenAsync();
			using var transaction = await connection.BeginTransactionAsync();

			// Insert comment into  DB
			var numRowUpdated = await connection.ExecuteAsync("INSERT INTO comments (UID, EventID, Text, Rating) VALUES (@UID, @EventID, @Text, @Rating)", comment);

			if (numRowUpdated == 0)
			{
				return null;
			}

			// Get last inserted ID
			var insertedId = await connection.QuerySingleAsync<int>("SELECT LAST_INSERT_ID()", transaction: transaction);

			await transaction.CommitAsync();

			return await GetById(insertedId);
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async Task<Comment?> GetById(int id)
	{
		using var connection = GetConnection();

		try
		{
			var comment = await connection.QueryFirstOrDefaultAsync<Comment>("SELECT * FROM comments WHERE CommentID = @CommentID", new { CommentID = id });
			return comment;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async Task<Comment?> UpdateById(int id, Comment comment)
	{
		using var connection = GetConnection();

		try
		{
			await connection.OpenAsync();
			using var transaction = await connection.BeginTransactionAsync();

			// Check if ID exists
			var foundComment = await connection.QueryFirstOrDefaultAsync<Comment>("SELECT * FROM comments WHERE CommentID = @CommentID", new { CommentID = id });

			if (foundComment == null)
			{
				return null;
			}

			foundComment.Text = comment.Text;
			foundComment.Rating = comment.Rating;
			foundComment.Timestamp = DateTime.UtcNow;

			// Update comment
			var numRowUpdated = await connection.ExecuteAsync("UPDATE comments SET Text = @Text, Rating = @Rating, Timestamp = @Timestamp WHERE CommentID = @CommentID", foundComment);

			if (numRowUpdated == 0)
			{
				return null;
			}

			await transaction.CommitAsync();

			return foundComment;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async Task<Comment?> Delete(int id)
	{
		using var connection = GetConnection();

		try
		{
			await connection.OpenAsync();
			using var transaction = await connection.BeginTransactionAsync();

			// Check if ID exists
			var foundComment = await connection.QueryFirstOrDefaultAsync<Comment>("SELECT * FROM comments WHERE CommentID = @CommentID", new { CommentID = id });

			if (foundComment == null)
			{
				return null;
			}

			// Delete comment from DB
			int numRowUpdated = await connection
				.ExecuteAsync("DELETE FROM comments WHERE CommentID = @CommentID", new { CommentID = id });

			if (numRowUpdated == 0)
			{
				return null;
			}

			await transaction.CommitAsync();

			return foundComment;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async Task<List<Comment>> GetAllByEventId(int eventId)
	{
		using var connection = GetConnection();

		try
		{
			var comments = await connection.QueryAsync<Comment>("SELECT * FROM comments WHERE EventID = @EventID", new { EventID = eventId });
			return comments.ToList();
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