using BasicDataLayers.DynamicStatements;
using BasicDataLayers.Entities;
using NUnit.Framework;
using System.Collections.Generic;

namespace BasicDataLayers.Tests
{
    /// <summary>
    /// These are bad tests, I just use these to run the code, not to assert anything.
    /// </summary>
    [TestFixture]
    public class BadTests
    {
        [Test]
        public void BuildStatements()
        {
            var e = new RudimentaryEntity();
            var lst = new List<RudimentaryEntity> {e};
            const string schema = "dbo";
            const string table = nameof(RudimentaryEntity);

            var bld = new AutoBuildDml();

            var insert = bld.GetInsertSql(lst, schema, table, nameof(RudimentaryEntity.PrimaryKey));

            var update = bld.GetUpdateSql(e, schema, table, nameof(RudimentaryEntity.PrimaryKey));

            Assert.Pass();
        }
    }
}
