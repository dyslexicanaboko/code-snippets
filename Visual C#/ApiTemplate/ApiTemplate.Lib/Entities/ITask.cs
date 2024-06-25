namespace ApiTemplate.Lib.Entities;

public interface ITask
{
  int TaskId { get; set; }

  int UserId { get; set; }

  int? CategoryId { get; set; }

  string Description { get; set; }

  bool IsFinished { get; set; }

  DateTime? FinishedOn { get; set; }

  DateTime CreatedOn { get; set; }

  DateTime? ModifiedOn { get; set; }
}
