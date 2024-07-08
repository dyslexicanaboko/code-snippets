using ApiTemplate.Lib.Entities;
using ApiTemplate.Lib.Services;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ApiTemplate.Lib.DataAccess
{
	public class TaskRepository
		: BaseRepository, ITaskRepository
	{
		public TaskRepository(IAppConfiguration configuration)
			: base(configuration)
		{
		}

		public async Task<TaskEntity?> Select(int taskId)
		{
			const string sql = @"
			SELECT
								TaskId,
								UserId,
								CategoryId,
								Description,
								IsFinished,
								FinishedOn,
								CreatedOn,
								ModifiedOn
			FROM dbo.Task
			WHERE TaskId = @TaskId";

			await using var connection = new SqlConnection(ConnectionString);

			var lst = (await connection.QueryAsync<TaskEntity>(sql, new { TaskId = taskId })).ToList();

			return lst.SingleOrDefault();
		}

		public async Task<IEnumerable<TaskEntity>> SelectByUserId(int userId)
		{
			const string sql = @"
			SELECT
								TaskId,
								UserId,
								CategoryId,
								Description,
								IsFinished,
								FinishedOn,
								CreatedOn,
								ModifiedOn
			FROM dbo.Task
			WHERE UserId = @UserId";

			await using var connection = new SqlConnection(ConnectionString);

			return await connection.QueryAsync<TaskEntity>(sql, new { UserId = userId });
		}

		public async Task<IEnumerable<TaskEntity>> SelectAll()
		{
			const string sql = @"
			SELECT
								TaskId,
								UserId,
								CategoryId,
								Description,
								IsFinished,
								FinishedOn,
								CreatedOn,
								ModifiedOn
			FROM dbo.Task";

			await using var connection = new SqlConnection(ConnectionString);

			return (await connection.QueryAsync<TaskEntity>(sql)).ToList();
		}

		//Preference on whether or not insert method returns a value is up to the user and the object being inserted
		public async Task<int> Insert(TaskEntity entity)
		{
			const string sql = @"INSERT INTO dbo.Task (
								UserId,
								CategoryId,
								Description,
								IsFinished,
								FinishedOn,
								CreatedOn,
								ModifiedOn
						) VALUES (
								@UserId,
								@CategoryId,
								@Description,
								@IsFinished,
								@FinishedOn,
								@CreatedOn,
								@ModifiedOn);

			SELECT SCOPE_IDENTITY() AS PK;";

			await using var connection = new SqlConnection(ConnectionString);

			var p = new DynamicParameters();
			p.Add("@UserId", dbType: DbType.Int32, value: entity.UserId);
			p.Add("@CategoryId", dbType: DbType.Int32, value: entity.CategoryId);

			p.Add(
				"@Description",
				dbType: DbType.AnsiString,
				value: entity.Description,
				size: 255);

			p.Add("@IsFinished", dbType: DbType.Boolean, value: entity.IsFinished);

			p.Add(
				"@FinishedOn",
				dbType: DbType.DateTime2,
				value: entity.FinishedOn,
				scale: 0);

			p.Add(
				"@CreatedOn",
				dbType: DbType.DateTime2,
				value: entity.CreatedOn,
				scale: 0);

			p.Add(
				"@ModifiedOn",
				dbType: DbType.DateTime2,
				value: entity.ModifiedOn,
				scale: 0);

			return await connection.ExecuteScalarAsync<int>(sql, entity);
		}

		public async Task Update(TaskEntity entity)
		{
			const string sql = @"UPDATE dbo.Task SET 
								UserId = @UserId,
								CategoryId = @CategoryId,
								Description = @Description,
								IsFinished = @IsFinished,
								FinishedOn = @FinishedOn,
								CreatedOn = @CreatedOn,
								ModifiedOn = @ModifiedOn
						WHERE TaskId = @TaskId";

			await using var connection = new SqlConnection(ConnectionString);

			var p = new DynamicParameters();
			p.Add("@TaskId", dbType: DbType.Int32, value: entity.TaskId);
			p.Add("@UserId", dbType: DbType.Int32, value: entity.UserId);
			p.Add("@CategoryId", dbType: DbType.Int32, value: entity.CategoryId);

			p.Add(
				"@Description",
				dbType: DbType.AnsiString,
				value: entity.Description,
				size: 255);

			p.Add("@IsFinished", dbType: DbType.Boolean, value: entity.IsFinished);

			p.Add(
				"@FinishedOn",
				dbType: DbType.DateTime2,
				value: entity.FinishedOn,
				scale: 0);

			p.Add(
				"@CreatedOn",
				dbType: DbType.DateTime2,
				value: entity.CreatedOn,
				scale: 0);

			p.Add(
				"@ModifiedOn",
				dbType: DbType.DateTime2,
				value: entity.ModifiedOn,
				scale: 0);

			await connection.ExecuteAsync(sql, p);
		}

		public async Task Delete(int taskId)
		{
			const string sql = "DELETE FROM dbo.Task WHERE TaskId = @TaskId";

			await using var connection = new SqlConnection(ConnectionString);
			
			await connection.ExecuteAsync(sql, taskId);
		}
	}
}
