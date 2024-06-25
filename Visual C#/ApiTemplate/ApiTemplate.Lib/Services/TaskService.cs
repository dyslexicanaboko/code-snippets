using ApiTemplate.Lib.DataAccess;
using ApiTemplate.Lib.Entities;
using ApiTemplate.Lib.Validation;

namespace ApiTemplate.Lib.Services
{
  public class TaskService
    : ITaskService
  {
    private readonly ITaskRepository _repoTask;
    private readonly ITaskValidation _validation;

    public TaskService(
      ITaskRepository repoTask,
      ITaskValidation validation)
    {
      _repoTask = repoTask;
      _validation = validation;
    }

    public TaskEntity? GetTask(int taskId)
    {
      Validations.IsGreaterThanZero(taskId, nameof(taskId));

      var dbEntity = _repoTask.Using(x => x.Select(taskId));

      return dbEntity;
    }

    public IList<TaskEntity> GetAllForUser(int userId)
    {
      Validations.ThrowOnError(
        () => Validations.IsUserIdValid(userId, false));

      var lst = _repoTask
        .Using(x => x.SelectByUserId(userId))
        .ToList();

      return lst;
    }

    public TaskEntity Add(TaskEntity? task)
    {
      Validations.IsValid(_validation, task, nameof(task));

      using (_repoTask)
      {
        task.TaskId = _repoTask.Insert(task);
      }

      return task;
    }

    public void Edit(TaskEntity task)
    {
      Validations.IsNotNull(task, nameof(task));

      using (_repoTask)
      {
        _repoTask.Update(task);
      }
    }

    public void Remove(int taskId)
    {
      Validations.IsGreaterThanZero(taskId, nameof(taskId));

      _repoTask.Using(x => x.Delete(taskId));
    }
  }
}
