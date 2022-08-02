using NUnit.Framework;

namespace EqualityAndComparison.Tests
{
    internal abstract class CompareTestBase<T>
        : TestBase
        where T : class, IEquatable<T>, new()
    {
        [Test]
        public virtual void Null_objects_are_equal()
        {
            //Arrange
            T left = null;
            T right = null;

            //Act / Assert
            AssertAreEqual(left, right);
        }

        [Test]
        public virtual void Left_object_is_null_then_not_equal()
        {
            //Arrange
            T left = null;
            var right = new T();

            //Act / Assert
            AssertAreNotEqual(left, right);
        }

        [Test]
        public virtual void Right_object_is_null_then_not_equal()
        {
            //Arrange
            var left = new T();
            T right = null;

            //Act / Assert
            AssertAreNotEqual(left, right);
        }

        [Test]
        public virtual void Empty_objects_are_equal()
        {
            //Arrange
            var left = new T();
            var right = new T();

            //Act / Assert
            AssertAreEqual(left, right);
        }

        [Test]
        public virtual void Non_empty_objects_are_equal()
        {
            //Arrange
            var left = GetFilledObject();
            var right = GetFilledObject();

            //Act / Assert
            AssertAreEqual(left, right);
        }

        protected abstract T GetFilledObject();

        public abstract void Objects_are_not_equal();
    }
}