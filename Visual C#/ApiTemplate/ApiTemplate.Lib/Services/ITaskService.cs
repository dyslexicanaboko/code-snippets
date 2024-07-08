using ApiTemplate.Lib.Entities;

namespace ApiTemplate.Lib.Services;

public interface ITaskService
{
  Task<TaskEntity?> GetTask(int taskId);

  Task<IList<TaskEntity>> GetAllForUser(int userId);

  Task<TaskEntity> Add(TaskEntity task);
  
  Task Edit(TaskEntity task);

  Task Remove(int taskId);
}
