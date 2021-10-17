/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
			   SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

IF NOT EXISTS(SELECT TOP(1) 1 FROM dbo.RudimentaryEntity WHERE PrimaryKey = 1)
BEGIN
	INSERT INTO dbo.RudimentaryEntity
	(
		ForeignKey
	   ,ReferenceId
	   ,IsYes
	   ,LuckyNumber
	   ,DollarAmount
	   ,MathCalculation
	   ,Label
	   ,RightNow
	)
	VALUES
	(   1             -- ForeignKey - int
	   ,NEWID()          -- ReferenceId - uniqueidentifier
	   ,1          -- IsYes - bit
	   ,7             -- LuckyNumber - int
	   ,124.67          -- DollarAmount - decimal(18, 2)
	   ,3.142738495893783929           -- MathCalculation - float
	   ,'Rudimentary entry'            -- Label - varchar(500)
	   ,SYSDATETIME() -- RightNow - datetime2(7)
	)
END