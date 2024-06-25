using FluentValidation.Results;

namespace ApiTemplate.Lib.Validation
{
  public interface IFluentValidation<in TEntity>
  {
    ValidationResult Validate(TEntity instance);
  }
}
