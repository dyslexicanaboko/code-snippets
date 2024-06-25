using FluentValidation.Results;

namespace ApiTemplate.Lib.Exceptions
{
  public sealed class InvalidArgumentException
    : BaseException
  {
    public InvalidArgumentException(string argument, string message, int errorCode)
      : base(message)
    {
      Argument = argument;

      ErrorCode = errorCode;
    }

    public InvalidArgumentException(ValidationFailure validationFailure)
      : base(validationFailure.ErrorMessage)
    {
      Argument = validationFailure.PropertyName;

      //This is an assumption I will never use non-numeric error codes
      ErrorCode = Convert.ToInt32(validationFailure.ErrorCode);
    }

    public string Argument { get; set; }
  }
}
