using ApiTemplate.Lib.Entities;

namespace ApiTemplate.Lib.DataAccess;

public interface ITaskRepository : IRepository
{
  Task<TaskEntity?> Select(int taskId);

  Task<IEnumerable<TaskEntity>> SelectByUserId(int userId);

  Task<IEnumerable<TaskEntity>> SelectAll();

  Task<int> Insert(TaskEntity entity);

  Task Update(TaskEntity entity);

  Task Delete(int taskId);
}
