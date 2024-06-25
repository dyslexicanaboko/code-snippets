using E = ApiTemplate.Lib.Exceptions.ErrorCodes.Unauthorized;

namespace ApiTemplate.Lib.Exceptions
{
  public static class Unauthorized
  {
    public static UnauthorizedException FailedAuthentication() => GetUnauthorized(
      "Authentication failed.",
      ErrorCodes.Unauthorized.FailedAuthentication);

    public static UnauthorizedException NotAuthenticated() => GetUnauthorized(
      "Not authenticated. Authenticate and try again.",
      ErrorCodes.Unauthorized.NotAuthenticated);

    public static UnauthorizedException InvalidPassword() => GetUnauthorized(
      "The provided username or password is incorrect.",
      ErrorCodes.Unauthorized.InvalidPassword);

    private static UnauthorizedException GetUnauthorized(string message, int errorCode)
      => new(message) { ErrorCode = errorCode };
  }
}
