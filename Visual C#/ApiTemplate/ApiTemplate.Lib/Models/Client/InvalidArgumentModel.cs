using ApiTemplate.Lib.Exceptions;

namespace ApiTemplate.Lib.Models.Client
{
  public class InvalidArgumentModel
    : ErrorModel
  {
    public InvalidArgumentModel(string field, string message, int errorCode)
      : base(message, errorCode)
      => Field = field;

    public InvalidArgumentModel(InvalidArgumentException exception)
      : this(exception.Argument, exception.Message, exception.ErrorCode)
    {
    }

    public string Field { get; set; }
  }
}
