using EqualityAndComparison.Lib.Entities;
using NUnit.Framework;

namespace EqualityAndComparison.Tests.Entities
{
    [TestFixture]
    public class ListEntityTests
        : CompareTestBase<ListEntity>
    {
        [Test]
        public override void Objects_are_not_equal()
        {
            //Arrange
            var left = GetFilledObject();

            var right = new ListEntity
            {
                CompoundEntities = new List<CompoundEntity>
                {
                    new CompoundEntity
                    {
                        Label = "Compounds daily",
                        NestedEntity = null
                    }
                },
                Sequence = new List<int> { 6, 6, 6 },
                SqlKeyWords = new List<string> { "HAVING", "JOIN", "FROM" }
            };

            //Act / Assert
            AssertAreNotEqual(left, right);
        }

        protected override ListEntity GetFilledObject()
        {
            var e = new ListEntity
            {
                CompoundEntities = new List<CompoundEntity>
                {
                    new CompoundEntity
                    {
                        Label = "Compounds daily",
                        NestedEntity = DummyData.GetFlatEntity()
                    }
                },
                Sequence = new List<int> { 0, 1, 2, 3, 4 },
                SqlKeyWords = new List<string> { "SELECT", "WHERE", "ORDER BY" }
            };

            return e;
        }
    }
}
