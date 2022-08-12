using NUnit.Framework;

namespace EqualityAndComparison.Tests
{
    public abstract class TestBase
    {
        protected void AssertAreEqual<T>(T left, T right)
            where T : class, IEquatable<T>, new()
        {
            //Act
            var areEqual = Equals(left, right); // left.Equals(right) and == won't work here

            //Assert
            Assert.IsTrue(areEqual);
        }

        protected void AssertAreNotEqual<T>(T left, T right)
            where T : class, IEquatable<T>, new()
        {
            //Act
            var areEqual = Equals(left, right); // left.Equals(right) and == won't work here

            //Assert
            Assert.IsFalse(areEqual);
        }
    }
}
