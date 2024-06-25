namespace ApiTemplate.Lib.Exceptions
{
  public sealed class ForbiddenException
    : BaseException
  {
    public ForbiddenException(string message)
      : base(message)
    {
    }

    /// <inheritdoc />
    public override int ErrorCode { get; set; } = 40300;
  }
}
