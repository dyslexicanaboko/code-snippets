using BR = ApiTemplate.Lib.Exceptions.ErrorCodes.BadRequest;

namespace ApiTemplate.Lib.Exceptions
{
  public static class InvalidArgument
  {
    public static InvalidArgumentException Symbol = new(
      "symbol",
      "Symbol cannot be null, blank or white space.",
      ErrorCodes.BadRequest.Symbol);

    public static InvalidArgumentException User = new("userId", "Invalid user.", ErrorCodes.BadRequest.User);

    public static InvalidArgumentException MalformedModel()
      => new("model", "The provided model is malformed.", ErrorCodes.BadRequest.MalformedModel);

    public static InvalidArgumentException Null(string argument)
      => new(argument, "The provided argument cannot be null.", ErrorCodes.BadRequest.Null);

    public static InvalidArgumentException Empty(string argument)
      => new(argument, "The provided argument cannot be empty.", ErrorCodes.BadRequest.Empty);

    public static InvalidArgumentException OutOfBounds(string argument, int lower, int upper)
      => new(argument, $"The provided argument must be between {lower} and {upper} inclusive.", ErrorCodes.BadRequest.OutOfBounds);

    public static InvalidArgumentException OutOfBounds(string argument, string enumeration)
      => new(
        argument,
        $"The provided argument must be found in the {enumeration} enumeration.",
        ErrorCodes.BadRequest.OutOfBoundsEnumeration);

    public static InvalidArgumentException NotGreaterThanZero(string argument)
      => new(argument, "The provided argument must be greater than zero.", ErrorCodes.BadRequest.NotGreaterThanZero);

    public static InvalidArgumentException NotGreaterThanDateTimeMin(string argument)
      => new(
        argument,
        $"The provided argument must be greater than {DateTime.MinValue}.",
        ErrorCodes.BadRequest.NotGreaterThanDateTimeMin);

    public static InvalidArgumentException StartDateGreaterThanEndDate(string argument)
      => new(
        argument,
        "The provided start date argument must be less than its end date.",
        ErrorCodes.BadRequest.StartDateGreaterThanEndDate);

    public static InvalidArgumentException EndDateLessThanStartDate(string argument)
      => new(
        argument,
        "The provided end date argument must be greater than its start date.",
        ErrorCodes.BadRequest.EndDateLessThanStartDate);
  }
}
