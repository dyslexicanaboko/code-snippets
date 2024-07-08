using ApiTemplate.Lib.DataAccess;
using ApiTemplate.Lib.Entities;
using ApiTemplate.Lib.Validation;

namespace ApiTemplate.Lib.Services
{
  public class TaskService
    : ITaskService
  {
    private readonly ITaskRepository _repository;
    private readonly ITaskValidation _validation;

    public TaskService(
      ITaskRepository repository,
      ITaskValidation validation)
    {
      _repository = repository;
      _validation = validation;
    }

    public async Task<TaskEntity?> GetTask(int taskId)
    {
      Validations.IsGreaterThanZero(taskId, nameof(taskId));

      var dbEntity = await _repository.Using(x => x.Select(taskId));

      return dbEntity;
    }

    public async Task<IList<TaskEntity>> GetAllForUser(int userId)
    {
      Validations.ThrowOnError(
        () => Validations.IsUserIdValid(userId, false));

      var lst = (await _repository
        .Using(x => x.SelectByUserId(userId)))
        .ToList();

      return lst;
    }

    public async Task<TaskEntity> Add(TaskEntity? task)
    {
      Validations.IsValid(_validation, task, nameof(task));

      using (_repository)
      {
        task.TaskId = await _repository.Insert(task);
      }

      return task;
    }

    public async Task Edit(TaskEntity task)
    {
      Validations.IsNotNull(task, nameof(task));

      await _repository.Using(x => x.Update(task));
    }

    public async Task Remove(int taskId)
    {
      Validations.IsGreaterThanZero(taskId, nameof(taskId));

      await _repository.Using(x => x.Delete(taskId));
    }
  }
}
