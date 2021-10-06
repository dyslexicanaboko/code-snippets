using BasicDataLayers.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace BasicDataLayers.DapperFlavor
{
    public class DapperRepository
		: BaseDal
	{
		public RudimentaryEntity Select(int primaryKey)
		{
			var sql = @"
			SELECT
				 DollarAmount
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

		public void Insert(RudimentaryEntity entity)
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
				,@RightNow)";

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(sql, entity);
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
                connection.Execute(sql, entity);
            }
		}
	}
}
