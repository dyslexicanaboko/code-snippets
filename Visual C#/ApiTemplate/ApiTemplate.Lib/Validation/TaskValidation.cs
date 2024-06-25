using ApiTemplate.Lib.Entities;
using FluentValidation;
using static ApiTemplate.Lib.Exceptions.InvalidArgument;

namespace ApiTemplate.Lib.Validation
{
  public interface ITaskValidation
    : IFluentValidation<TaskEntity>
  {
  }

  public class TaskValidation
    : AbstractValidator<TaskEntity>, ITaskValidation
  {
    public TaskValidation()
    {
      RuleFor(r => r.UserId)
        .GreaterThan(0)
        .WithMessageAndErrorCode(User);

      RuleFor(r => r.TaskId)
        .GreaterThan(0)
        .WithMessageAndErrorCode(NotGreaterThanZero(nameof(TaskEntity.TaskId)));
    }
  }
}
