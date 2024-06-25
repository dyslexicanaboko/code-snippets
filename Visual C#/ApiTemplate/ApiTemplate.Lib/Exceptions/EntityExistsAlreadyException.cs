namespace ApiTemplate.Lib.Exceptions
{
  public abstract class EntityExistsAlreadyException
    : BaseException
  {
    protected EntityExistsAlreadyException(string message)
      : base(message)
    {
    }

    public override int ErrorCode { get; set; } = ErrorCodes.Errors.EntityExistsAlready;
  }
}
