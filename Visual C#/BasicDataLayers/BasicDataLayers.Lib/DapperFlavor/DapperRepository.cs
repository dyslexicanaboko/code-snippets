using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BasicDataLayers.Lib.Entities;

namespace BasicDataLayers.Lib.DapperFlavor
{
	public class DapperRepository
		: BaseDal
	{
		public RudimentaryEntity Select(int primaryKey)
		{
			var sql = @"
			SELECT
				 PrimaryKey
				,DollarAmount
				,ForeignKey
				,IsYes
				,Label
				,LuckyNumber
				,MathCalculation
				,ReferenceId
				,RightNow
			FROM dbo.RudimentaryEntity
			WHERE PrimaryKey = @PrimaryKey";

			using (var connection = new SqlConnection(ConnectionString))
			{
				var lst = connection.Query<RudimentaryEntity>(sql, new { PrimaryKey = primaryKey}).ToList();

				if (!lst.Any()) return null;

				var entity = lst.Single();

				return entity;
			}
		}

		public IEnumerable<RudimentaryEntity> SelectAll()
		{
			var sql = @"
			SELECT
				 PrimaryKey
				,DollarAmount
				,ForeignKey
				,IsYes
				,Label
				,LuckyNumber
				,MathCalculation
				,ReferenceId
				,RightNow
			FROM dbo.RudimentaryEntity";

			using (var connection = new SqlConnection(ConnectionString))
			{
				var lst = connection.Query<RudimentaryEntity>(sql).ToList();

				return lst;
			}
		}

		public int Insert(RudimentaryEntity entity)
		{
			var sql = @"INSERT INTO dbo.RudimentaryEntity (
				 DollarAmount
				,ForeignKey
				,IsYes
				,Label
				,LuckyNumber
				,MathCalculation
				,ReferenceId
				,RightNow) VALUES (
				 @DollarAmount
				,@ForeignKey
				,@IsYes
				,@Label
				,@LuckyNumber
				,@MathCalculation
				,@ReferenceId
				,@RightNow);

				SELECT SCOPE_IDENTITY() AS PK;";

			using (var connection = new SqlConnection(ConnectionString))
			{
				return connection.ExecuteScalar<int>(sql, entity);
			}
		}

		public void Update(RudimentaryEntity entity)
		{
			var sql = @"UPDATE dbo.RudimentaryEntity SET 
						 DollarAmount = @DollarAmount
						,ForeignKey = @ForeignKey
						,IsYes = @IsYes
						,Label = @Label
						,LuckyNumber = @LuckyNumber
						,MathCalculation = @MathCalculation
						,ReferenceId = @ReferenceId
						,RightNow = @RightNow
					WHERE PrimaryKey = @PrimaryKey";

			using (var connection = new SqlConnection(ConnectionString))
			{
				var p = new DynamicParameters();
				//Sizes should be filled in where possible because it WILL make a difference depending on the parameter type
				//DateTime2 for example will not get the precision desired if you do not put in the number for precision
				p.Add("@PrimaryKey", dbType: DbType.Int32, value: entity.PrimaryKey);
				p.Add("@ForeignKey", dbType: DbType.Int32, value: entity.ForeignKey);
				p.Add("@ReferenceId", dbType: DbType.Guid, value: entity.ReferenceId);
				p.Add("@IsYes", dbType: DbType.Boolean, value: entity.IsYes);
				p.Add("@LuckyNumber", dbType: DbType.Int32, value: entity.LuckyNumber);
				p.Add("@DollarAmount", dbType: DbType.Decimal, value: entity.DollarAmount);
				p.Add("@MathCalculation", dbType: DbType.Double, value: entity.MathCalculation);
				p.Add("@Label", dbType: DbType.String, value: entity.Label);
				p.Add("@RightNow", dbType: DbType.DateTime2, size: 7, value: entity.RightNow);

				connection.Execute(sql, p);
			}
		}
	}
}
