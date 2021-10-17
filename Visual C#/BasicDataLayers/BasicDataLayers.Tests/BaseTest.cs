using BasicDataLayers.Lib;
using BasicDataLayers.Lib.Entities;
using NUnit.Framework;
using System;
using System.Data;

namespace BasicDataLayers.Tests
{
    public abstract class BaseTest
        : BaseDal
    {
        protected const string Schema = "dbo";
        protected const string Table = nameof(RudimentaryEntity);

        /*
        using (IDataReader dr = ExecuteReaderText(insert.Sql, insert.Parameters))
        {
            var pk = Convert.ToInt32(GetScalar(dr, "PK"));
        }
         */

        protected T Using<T>(Func<IDataReader> method, Func<T> method2)
        {
            using (IDataReader dr = method())
            {
                return method2();
            }
        }

        //protected object ExecuteScalar(string sql, params SqlParameter[] parameters)
        //{
        //    using (var dr = ExecuteReaderText(sql, parameters))
        //    {
        //        return GetScalar(dr, "PK");
        //    }
        //}

        protected RudimentaryEntity ToEntity(IDataReader reader)
        {
            var r = reader;

            var e = new RudimentaryEntity();
            e.PrimaryKey = Convert.ToInt32(r["PrimaryKey"]);
            e.DollarAmount = Convert.ToDecimal(r["DollarAmount"]);
            e.ForeignKey = Convert.ToInt32(r["ForeignKey"]);
            e.IsYes = Convert.ToBoolean(r["IsYes"]);
            e.Label = Convert.ToString(r["Label"]);
            e.LuckyNumber = Convert.ToInt32(r["LuckyNumber"]);
            e.MathCalculation = Convert.ToDouble(r["MathCalculation"]);
            e.ReferenceId = Guid.Parse(Convert.ToString(r["ReferenceId"]));
            e.RightNow = Convert.ToDateTime(r["RightNow"]);

            return e;
        }

        protected void AssertAreEqual(RudimentaryEntity e, RudimentaryEntity a)
        {
            Assert.AreEqual(e.PrimaryKey, a.PrimaryKey);
            Assert.AreEqual(e.ForeignKey, a.ForeignKey);
            Assert.AreEqual(e.ReferenceId, a.ReferenceId);
            Assert.AreEqual(e.IsYes, a.IsYes);
            Assert.AreEqual(e.LuckyNumber, a.LuckyNumber);
            Assert.AreEqual(e.DollarAmount, a.DollarAmount);
            Assert.AreEqual(e.MathCalculation, a.MathCalculation);
            Assert.AreEqual(e.Label, a.Label);
            Assert.AreEqual(e.RightNow, a.RightNow);
        }
    }
}
