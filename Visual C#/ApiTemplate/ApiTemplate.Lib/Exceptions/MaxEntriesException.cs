namespace ApiTemplate.Lib.Exceptions
{
  public class MaxEntriesException
    : BaseException
  {
    public MaxEntriesException(string symbol, string subject, int max)
      : base(GetMessage(symbol, subject, max))
    {
    }

    public override int ErrorCode { get; set; } = ErrorCodes.Errors.MaxEntries;

    private static string GetMessage(string symbol, string subject, int max)
    {
      var str = $"Stock `{symbol}` already has a max of {max} {subject} entries.";

      return str;
    }
  }
}
