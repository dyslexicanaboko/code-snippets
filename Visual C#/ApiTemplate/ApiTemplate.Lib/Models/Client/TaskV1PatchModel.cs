using ApiTemplate.Lib.Entities;

namespace ApiTemplate.Lib.Models.Client
{
  public class TaskV1PatchModel
  {
    public TaskV1PatchModel(ITask task)
    {
      UserId = task.UserId;
      CategoryId = task.CategoryId;
      Description = task.Description;
    }

    public int UserId { get; set; }

    public int? CategoryId { get; set; }

    public string Description { get; set; }
  }
}
