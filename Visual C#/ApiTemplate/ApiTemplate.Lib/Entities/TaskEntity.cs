using ApiTemplate.Lib.Models.Client;

namespace ApiTemplate.Lib.Entities
{
  public class TaskEntity
      : ITask, IEquatable<TaskEntity>
  {
    public TaskEntity()
    {

    }

    public TaskEntity(int userId, TaskV1CreateModel model)
    {
      UserId = userId;
      CategoryId = model.CategoryId;
      Description = model.Description;
    }

    public TaskEntity(int userId, TaskV1PatchModel model)
    {
      UserId = userId;
      CategoryId = model.CategoryId;
      Description = model.Description;
    }

    public int TaskId { get; set; }

    public int UserId { get; set; }

    public int? CategoryId { get; set; }

    public string Description { get; set; }

    public bool IsFinished { get; set; }

    public DateTime? FinishedOn { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public override bool Equals(object? obj) => this.Equals(obj as TaskEntity);

    public bool Equals(TaskEntity? other)
    {
      if (other is null) return false;

      if (object.ReferenceEquals(this, other)) return true;

      if (this.GetType() != other.GetType()) return false;

      return
        TaskId == other.TaskId &&
        UserId == other.UserId &&
        CategoryId == other.CategoryId &&
        Description == other.Description &&
        IsFinished == other.IsFinished &&
        FinishedOn == other.FinishedOn &&
        CreatedOn == other.CreatedOn &&
        ModifiedOn == other.ModifiedOn;
    }

    public override int GetHashCode() =>
      TaskId.GetHashCode() +
      UserId.GetHashCode() +
      CategoryId.GetHashCode() +
      Description.GetHashCode() +
      IsFinished.GetHashCode() +
      FinishedOn.GetHashCode() +
      CreatedOn.GetHashCode() +
      ModifiedOn.GetHashCode();

    public static bool operator ==(TaskEntity? lhs, TaskEntity? rhs)
    {
      if (lhs is not null) return lhs.Equals(rhs);

      return rhs is null;
    }

    public static bool operator !=(TaskEntity? lhs, TaskEntity? rhs) => !(lhs == rhs);
  }
}
