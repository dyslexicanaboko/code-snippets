using EqualityAndComparison.Lib.Entities;
using NUnit.Framework;

namespace EqualityAndComparison.Tests.Entities
{
    [TestFixture]
    public class FlatEntityTests
        : CompareTestBase<FlatEntity>
    {
        [Test]
        public override void Objects_are_not_equal()
        {
            //Arrange
            var left = GetFilledObject();

            var right = new FlatEntity
            {
                Age = 34,
                JobCode = "Good Manager",
                Price = 160000.42M,
                PrimaryKey = new Guid("{87A09E8B-2835-4C8C-A48E-42FD4A226D85}"),
                SignificantDate = new DateTime(2000, 06, 29)
            };

            //Act / Assert
            AssertAreNotEqual(left, right);
        }

        protected override FlatEntity GetFilledObject()
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
