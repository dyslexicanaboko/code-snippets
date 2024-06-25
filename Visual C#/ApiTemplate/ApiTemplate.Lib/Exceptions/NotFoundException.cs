namespace ApiTemplate.Lib.Exceptions;

public class NotFoundException
  : BaseException
{
  public NotFoundException(string message)
    : base(message)
  {
  }

  public override int ErrorCode { get; set; } = 40400;
}
