using Microsoft.Data.SqlClient;

namespace ApiTemplate.Lib.DataAccess
{
  public interface IRepository<T>
    : IRepository
    where T : class, new()
  {
    T? Select(int earningsId);

    IEnumerable<T> SelectAll();

    int Insert(T entity);

    void Update(T entity);
  }

  public interface IRepository
    : IDisposable
  {
    //This exists just to unify all repositories as being "a type of" repository
    void SetTransaction(SqlTransaction transaction);
  }
}
