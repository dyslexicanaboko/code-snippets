using EqualityAndComparison.Lib.Entities;
using NUnit.Framework;

namespace EqualityAndComparison.Tests.Entities
{
    [TestFixture]
    public class CompoundEntityTests
        : CompareTestBase<CompoundEntity>
    {
        [Test]
        public override void Objects_are_not_equal()
        {
            //Arrange
            var left = GetFilledObject();

            var right = new CompoundEntity
            {
                Label = "LABEL",
                NestedEntity = DummyData.GetFlatEntity()
            };

            right.NestedEntity.Age = 200;
            right.NestedEntity.JobCode = "job code";

            //Act / Assert
            AssertAreNotEqual(left, right);
        }

        protected override CompoundEntity GetFilledObject()
        {
            var e = new CompoundEntity
            {
                Label = "label",
                NestedEntity = DummyData.GetFlatEntity()
            };

            return e;
        }
    }
}
