using EqualityAndComparison.Lib.Entities;

namespace EqualityAndComparison.Tests
{
    public static class DummyData
    {
        public static FlatEntity GetFlatEntity()
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
    }
}
