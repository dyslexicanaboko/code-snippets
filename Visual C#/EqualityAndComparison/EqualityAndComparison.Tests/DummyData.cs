using EqualityAndComparison.Lib.Entities;

namespace EqualityAndComparison.Tests
{
  public static class DummyData
  {
    public static FlatEntity GetFlatEntity()
      => GetMediocreManager();

    public static FlatEntity GetMediocreManager()
    {
      var e = new FlatEntity
      {
        Age = 8,
        JobCode = "Mediocre Manager",
        Price = 10.57M,
        PrimaryKey = new Guid("{439F97C1-B2F7-4464-8BE2-DEF386964BE9}"),
        SignificantDate = new DateTime(1998, 03, 31)
      };

      return e;
    }
    
    public static FlatEntity GetBrainDeadEmployee() => new()
      {
        Age = 9,
        JobCode = "Brain dead employee",
        Price = 0.57M,
        PrimaryKey = new Guid("{6408F671-B08F-4413-BC71-EE4AA1F4212F}"),
        SignificantDate = new DateTime(1999, 04, 01)
      };

    public static FlatEntity GetMoronicCLevel() => new()
    {
      Age = 10,
      JobCode = "Moronic C-Level",
      Price = 5000.69M,
      PrimaryKey = new Guid("{8DEB7558-711D-4D91-A74F-D41E84FC07CE}"),
      SignificantDate = new DateTime(1954, 12, 12)
    };

    public static List<FlatEntity> GetTwoFlatEntities() => new()
    {
      GetBrainDeadEmployee(),
      GetMoronicCLevel()
    };

    public static List<FlatEntity> GetThreeFlatEntities()
    {
      //Don't change the order, I want to preserve the indexing
      var lst = new List<FlatEntity>(3) { GetMediocreManager() };
      
      lst.AddRange(GetTwoFlatEntities());

      return lst;
    }

    public static List<FlatEntity> GetSortedFlatEntities() => new()
    {
        GetBrainDeadEmployee(),
        GetMediocreManager(),
        GetMoronicCLevel()
    };

    public static List<DumbEntity> ToDumbEntities(this List<FlatEntity> entities)
      => entities.Select(x => x.ToDumbEntity()).ToList();

    public static DumbEntity ToDumbEntity(this IFlatEntity entity)
      => new(entity);
  }
}
