using CommunityToolkit.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using ApiTemplate.Lib.Exceptions;
using static ApiTemplate.Lib.Exceptions.InvalidArgument;

namespace ApiTemplate.Lib.Validation
{
  /// <summary>
  /// Common case validations which are used when `FluentValidation` is too cumbersome.
  /// </summary>
  public static class Validations
  {
    public static InvalidArgumentException? IsUserIdValid(int userId, bool raiseException = true)
      => TestArgument(
        () => Guard.IsGreaterThan(userId, 0),
        raiseException,
        User);

    public static InvalidArgumentException? IsNotNull<T>([NotNull] T? value, string argument, bool raiseException = true)
#pragma warning disable CS8777
      => TestArgument(
        () => Guard.IsNotNull(value),
        raiseException,
        Null(argument));
#pragma warning restore CS8777

    public static InvalidArgumentException? IsNotEmpty<T>(
      ICollection<T> collection,
      string argument,
      bool raiseException = true)
      => TestArgument(
        () => Guard.IsNotEmpty(collection),
        raiseException,
        Null(argument));

    public static InvalidArgumentException? IsGreaterThanZero(int value, string argument, bool raiseException = true)
      => TestArgument(
        () => Guard.IsGreaterThan(value, 0),
        raiseException,
        NotGreaterThanZero(argument));

    //Guard.IsGreaterThanOrEqualTo(position.DateClosed.Value, position.DateOpened, nameof(position.DateClosed));
    public static InvalidArgumentException? IsEndDateGreaterThanStartDate(DateTime endDate, DateTime startDate, string argument, bool raiseException = true)
      => TestArgument(
        () => Guard.IsGreaterThanOrEqualTo(endDate, startDate),
        raiseException,
        EndDateLessThanStartDate(argument));

    public static BadRequestException IsMalformedModel() => new (MalformedModel());

    public static void ThrowOnError(params Func<InvalidArgumentException?>[] tests)
    {
      var lst = tests
        .Select(test => test())
        .Where(ex => ex != null)
        .ToList();

      if (!lst.Any()) return;

      throw new BadRequestException(lst!);
    }

    public static void IsValid<TEntity>(IFluentValidation<TEntity> validation, [NotNull]TEntity? entity, string entityName)
    {
      IsNotNull(entity, entityName);

      var result = validation.Validate(entity);

      if (!result.IsValid) throw new BadRequestException(result.Errors);
    }

    private static InvalidArgumentException? TestArgument(
      Action test,
      bool raiseException,
      InvalidArgumentException exception)
    {
      //This is a hack until I come up with a better way to handle this.
      //I like the Guard syntax, but it automatically raises exceptions.
      try
      {
        test();

        return null;
      }
      catch
      {
        if (raiseException) throw exception;

        return exception;
      }
    }
  }
}
