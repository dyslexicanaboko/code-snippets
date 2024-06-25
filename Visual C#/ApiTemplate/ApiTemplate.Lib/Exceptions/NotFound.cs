namespace ApiTemplate.Lib.Exceptions;

public static class NotFound
{
  public static NotFoundException Task(int id) => GetNotFound("Task", id, ErrorCodes.NotFound.Task);

  public static NotFoundException User(int id) => GetNotFound("User", id, ErrorCodes.NotFound.User);

  public static NotFoundException UserCredentials()
    => GetNotFound("User not found", ErrorCodes.NotFound.UserCredentials);

  private static NotFoundException GetNotFound(string subject, int id, int errorCode) => GetNotFound(
    $"{subject} with id `{id}` could not be found.",
    errorCode);

  private static NotFoundException GetNotFound(string subject, string symbol, int errorCode) => GetNotFound(
    $"A {subject} with symbol `{symbol}` could not be found.",
    errorCode);

  private static NotFoundException GetNotFound(string message, int errorCode) => new(message) { ErrorCode = errorCode };
}
