CREATE TABLE [dbo].[RudimentaryEntity]
(
	 PrimaryKey int IDENTITY(1, 1) NOT NULL
	,ForeignKey int NOT NULL
	,ReferenceId UNIQUEIDENTIFIER NOT NULL
	,IsYes BIT NOT NULL
	,LuckyNumber int NOT NULL
	,DollarAmount decimal(18, 2) NOT NULL
	,MathCalculation float NOT NULL
	,Label VARCHAR(500) NOT NULL
	,RightNow DateTime2(7) NOT NULL
)
