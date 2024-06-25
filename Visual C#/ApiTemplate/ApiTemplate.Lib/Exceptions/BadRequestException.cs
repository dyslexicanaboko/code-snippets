using FluentValidation.Results;

namespace ApiTemplate.Lib.Exceptions
{
  public class BadRequestException
    : BaseException
  {
    private const string ErrorMessage = "One or more arguments are invalid.";

    public BadRequestException()
      : base(ErrorMessage)
    {
    }

    public BadRequestException(InvalidArgumentException exception)
      : this()
    {
      InvalidArguments.Add(exception);
    }

    public BadRequestException(IEnumerable<InvalidArgumentException> exception)
      : this()
    {
      InvalidArguments.AddRange(exception);
    }

    public BadRequestException(IEnumerable<ValidationFailure> validationFailures)
      : this()
    {
      InvalidArguments.AddRange(validationFailures.Select(x => new InvalidArgumentException(x)));
    }

    /// <inheritdoc />
    public override int ErrorCode { get; set; } = 40000;

    public List<InvalidArgumentException> InvalidArguments { get; set; } = new();
  }
}
