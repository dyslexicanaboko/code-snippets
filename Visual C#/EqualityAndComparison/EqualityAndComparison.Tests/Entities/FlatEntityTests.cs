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

        protected override FlatEntity GetFilledObject() => DummyData.GetFlatEntity();
    }
}
