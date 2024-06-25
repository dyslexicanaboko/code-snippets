namespace ApiTemplate.Lib.Exceptions
{
  public sealed class UnauthorizedException
    : BaseException
  {
    public UnauthorizedException(string message)
      : base(message)
    {
    }

    /// <inheritdoc />
    public override int ErrorCode { get; set; } = 40100;
  }
}
