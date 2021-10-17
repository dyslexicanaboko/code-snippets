using BasicDataLayers.Lib.Entities;
using BasicDataLayers.Lib.StaticStatements;
using NUnit.Framework;
using System;

namespace BasicDataLayers.Tests
{
    [TestFixture]
    public class StaticStatementsTests
        : BaseTest
    {
        [Test]
        public void InsertUpdateSelect()
        {
            var e = new RudimentaryEntity();
            var repo = new RudimentaryRepository();

            var pk = repo.Insert(e);

            e.PrimaryKey = pk;
            e.ForeignKey = 20;
            e.ReferenceId = Guid.NewGuid();
            e.IsYes = !e.IsYes;
            e.LuckyNumber = 8;
            e.DollarAmount = 255.67M;
            e.MathCalculation = new Random().NextDouble();
            e.Label = "Updating what was inserted";
            e.RightNow = DateTime.UtcNow;

            repo.Update(e);

            var a = repo.Select(e.PrimaryKey);

            AssertAreEqual(e, a);
        }
    }
}
