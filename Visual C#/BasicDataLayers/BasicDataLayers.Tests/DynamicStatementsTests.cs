using BasicDataLayers.Lib.DynamicStatements;
using BasicDataLayers.Lib.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BasicDataLayers.Tests
{
    [TestFixture]
    public class DynamicStatementsTests
        : BaseTest
    {
        [Test]
        public void InsertUpdateSelect()
        {
            var e = new RudimentaryEntity();
            var lst = new List<RudimentaryEntity> { e };
            var bld = new AutoBuildDml();

            var insert = bld.GetInsertSql(lst, Schema, Table, nameof(RudimentaryEntity.PrimaryKey));

            insert.Sql += "; SELECT SCOPE_IDENTITY() AS PK;";

            int pk;

            using (var dr = ExecuteReaderText(insert.Sql, insert.Parameters))
            {
                pk = Convert.ToInt32(GetScalar(dr, "PK"));
            }

            e.PrimaryKey = pk;
            e.ForeignKey = 20;
            e.ReferenceId = Guid.NewGuid();
            e.IsYes = !e.IsYes;
            e.LuckyNumber = 8;
            e.DollarAmount = 255.67M;
            e.MathCalculation = new Random().NextDouble();
            e.Label = "Updating what was inserted";
            e.RightNow = DateTime.UtcNow;

            var update = bld.GetUpdateSql(e, Schema, Table, nameof(RudimentaryEntity.PrimaryKey));

            bld.ExecuteNonQuery(update);

            var select = bld.GetSelectByPrimaryKeySql<RudimentaryEntity>(
                    Schema, 
                    Table, 
                    nameof(RudimentaryEntity.PrimaryKey),
                    pk);

            RudimentaryEntity a;

            using (var dr = ExecuteReaderText(select.Sql, select.Parameters))
            {
                dr.Read();

                a = ToEntity(dr);
            }

            AssertAreEqual(e, a);
        }
    }
}
