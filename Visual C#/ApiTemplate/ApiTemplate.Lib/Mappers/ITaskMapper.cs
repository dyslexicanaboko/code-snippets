using ApiTemplate.Lib.Entities;
using ApiTemplate.Lib.Models;
using ApiTemplate.Lib.Models.Client;

namespace ApiTemplate.Lib.Mappers;

public interface ITaskMapper
{
  TaskEntity ToEntity(TaskModel model);

  TaskEntity ToEntity(ITask target);

  TaskModel ToModel(TaskEntity entity);

  TaskModel ToModel(ITask target);

  TaskEntity? ToEntity(int userId, TaskV1CreateModel? model);

  TaskV1PatchModel? ToPatchModel(TaskEntity? entity);

  TaskEntity ToEntity(int userId, TaskV1PatchModel model);
}
