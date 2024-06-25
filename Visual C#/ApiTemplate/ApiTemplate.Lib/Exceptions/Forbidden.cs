using E = ApiTemplate.Lib.Exceptions.ErrorCodes.Forbidden;

namespace ApiTemplate.Lib.Exceptions
{
  public static class Forbidden
  {
    public static ForbiddenException AccessDenied() => GetForbidden(
      "You do not have access to this resource.",
      ErrorCodes.Forbidden.AccessDenied);

    private static ForbiddenException GetForbidden(string message, int errorCode)
      => new(message) { ErrorCode = errorCode };
  }
}
