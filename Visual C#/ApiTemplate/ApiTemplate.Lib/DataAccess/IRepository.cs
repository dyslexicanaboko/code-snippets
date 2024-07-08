using Microsoft.Data.SqlClient;

namespace ApiTemplate.Lib.DataAccess
{
  public interface IRepository<T>
    : IRepository
    where T : class, new()
  {
    Task<T?> Select(int earningsId);

    Task<IEnumerable<T>> SelectAll();

    Task<int> Insert(T entity);

    Task Update(T entity);
  }

  public interface IRepository
    : IDisposable
  {
    //This exists just to unify all repositories as being "a type of" repository
    void SetTransaction(SqlTransaction transaction);
  }
}
