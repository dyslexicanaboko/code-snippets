using System.Data;
using ApiTemplate.Lib.Entities;
using ApiTemplate.Lib.Services;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ApiTemplate.Lib.DataAccess
{
	public class TaskRepository
		: BaseRepository, ITaskRepository
	{
		public TaskRepository(IAppConfiguration configuration)
			: base(configuration)
		{
		}

		public TaskEntity? Select(int taskId)
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

			using var connection = new SqlConnection(ConnectionString);

			var lst = connection.Query<TaskEntity>(sql, new { TaskId = taskId }).ToList();

			return lst.SingleOrDefault();
		}

		public IEnumerable<TaskEntity> SelectByUserId(int userId)
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

			using var connection = new SqlConnection(ConnectionString);

			return connection.Query<TaskEntity>(sql, new { UserId = userId });
		}

		public IEnumerable<TaskEntity> SelectAll()
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

			using var connection = new SqlConnection(ConnectionString);

			return connection.Query<TaskEntity>(sql).ToList();
		}

		//Preference on whether or not insert method returns a value is up to the user and the object being inserted
		public int Insert(TaskEntity entity)
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

			using var connection = new SqlConnection(ConnectionString);

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

			return connection.ExecuteScalar<int>(sql, entity);
		}

		public void Update(TaskEntity entity)
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

			using var connection = new SqlConnection(ConnectionString);

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

			connection.Execute(sql, p);
		}

		public void Delete(int taskId)
		{
			const string sql = "DELETE FROM dbo.Task WHERE TaskId = @TaskId";

			using var connection = new SqlConnection(ConnectionString);

			var p = new DynamicParameters();
			p.Add("@TaskId", dbType: DbType.Int32, value: taskId);

			connection.Execute(sql, p);
		}
	}
}
