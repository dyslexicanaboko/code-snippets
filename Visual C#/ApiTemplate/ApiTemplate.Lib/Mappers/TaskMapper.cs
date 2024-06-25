using ApiTemplate.Lib.Entities;
using ApiTemplate.Lib.Models;
using ApiTemplate.Lib.Models.Client;

namespace ApiTemplate.Lib.Mappers
{
  public class TaskMapper
    : BaseMapper, ITaskMapper
  {
    public TaskEntity ToEntity(TaskModel model)
    {
      var entity = new TaskEntity();
      entity.TaskId = model.TaskId;
      entity.UserId = model.UserId;
      entity.CategoryId = model.CategoryId;
      entity.Description = model.Description;
      entity.IsFinished = model.IsFinished;
      entity.FinishedOn = model.FinishedOn;
      entity.CreatedOn = model.CreatedOn;
      entity.ModifiedOn = model.ModifiedOn;

      return entity;
    }

    public TaskEntity? ToEntity(int userId, TaskV1CreateModel? model)
      => model == null ? null : new TaskEntity(userId, model);

    public TaskV1PatchModel? ToPatchModel(TaskEntity? model)
      => model == null ? null : new TaskV1PatchModel(model);

    public TaskEntity ToEntity(int userId, TaskV1PatchModel model)
      => new (userId, model);

    public TaskModel ToModel(TaskEntity entity)
    {
      var model = new TaskModel();
      model.TaskId = entity.TaskId;
      model.UserId = entity.UserId;
      model.CategoryId = entity.CategoryId;
      model.Description = entity.Description;
      model.IsFinished = entity.IsFinished;
      model.FinishedOn = entity.FinishedOn;
      model.CreatedOn = entity.CreatedOn;
      model.ModifiedOn = entity.ModifiedOn;

      return model;
    }

    public TaskEntity ToEntity(ITask target)
    {
      var entity = new TaskEntity();
      entity.TaskId = target.TaskId;
      entity.UserId = target.UserId;
      entity.CategoryId = target.CategoryId;
      entity.Description = target.Description;
      entity.IsFinished = target.IsFinished;
      entity.FinishedOn = target.FinishedOn;
      entity.CreatedOn = target.CreatedOn;
      entity.ModifiedOn = target.ModifiedOn;

      return entity;
    }

    public TaskModel ToModel(ITask target)
    {
      var model = new TaskModel();
      model.TaskId = target.TaskId;
      model.UserId = target.UserId;
      model.CategoryId = target.CategoryId;
      model.Description = target.Description;
      model.IsFinished = target.IsFinished;
      model.FinishedOn = target.FinishedOn;
      model.CreatedOn = target.CreatedOn;
      model.ModifiedOn = target.ModifiedOn;

      return model;
    }
  }
}
