namespace ApiTemplate.Lib.Mappers
{
  public abstract class BaseMapper
  {
    protected IList<TModel> ToList<TEntity, TModel>(IList<TEntity>? target, Func<TEntity?, TModel?> mapper)
      where TEntity : class, new()
      where TModel : class //Model will not have an empty constructor on purpose
    {
      if (target == null || !target.Any()) return new List<TModel>();

      var lst = target.Select(mapper).ToList();

      return lst!;
    }

    protected IList<TEntity> ToList<TModel, TEntity>(int userId, IList<TModel>? target, Func<int, TModel?, TEntity?> mapper)
      where TEntity : class, new()
      where TModel : class //Model will not have an empty constructor on purpose
    {
      if (target == null || !target.Any()) return new List<TEntity>();

      var lst = target.Select(x => mapper(userId, x)).ToList();

      return lst!;
    }
  }
}
