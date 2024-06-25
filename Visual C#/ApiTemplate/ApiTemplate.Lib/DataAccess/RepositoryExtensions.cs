namespace ApiTemplate.Lib.DataAccess
{
  public static class RepositoryExtensions
  {
    public static TReturn Using<TRepo, TReturn>(this TRepo repo, Func<TRepo, TReturn> method)
      where TRepo : IRepository
    {
      using (repo)
      {
        return method(repo);
      }
    }

    public static void Using<TRepo>(this TRepo repo, Action<TRepo> method)
      where TRepo : IRepository
    {
      using (repo)
      {
        method(repo);
      }
    }
  }
}
