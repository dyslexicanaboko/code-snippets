/*
If it isn't in source control, it should not be in production.
This script is designed to drop objects in order so that their common parent object will be dropped too.
The order of the execution matters otherwise the script could fail.
Isolation level is serializable on purpose.

The data shown below are all dummy objects.
*/

PRINT '==================================================================='
PRINT 'Dropping unversioned objects'
PRINT '==================================================================='
	
-- List of stored procedures, simple drop statements, may or may not exist
DECLARE @tblDropStatements TABLE
(
	 DropStatement VARCHAR(2000) NOT NULL
	,ExecutionOrder INT NOT NULL
);
	
PRINT '-------------------------------------------------------------------'
PRINT 'Getting functions'
PRINT '-------------------------------------------------------------------'

INSERT INTO @tblDropStatements
(
	 DropStatement
	,ExecutionOrder
)
SELECT DISTINCT
	 CONCAT('DROP FUNCTION [', s.name, '].[',f.name,']') AS FnDropStatement
	,0
FROM sys.schemas s
	INNER JOIN sys.objects f
		ON f.schema_id = s.schema_id
WHERE 
		f.type IN ('FN', 'IF', 'TF') -- All function types
	AND s.name = 'dbo' 
	AND f.name IN 
( 
	 'fn_myFunction'
	,'GetScalarValue'
	,'func2'
)

PRINT '-------------------------------------------------------------------'
PRINT 'Getting stored procedures'
PRINT '-------------------------------------------------------------------'

-- List of stored procedures, simple drop statements, may or may not exist
INSERT INTO @tblDropStatements
(
	 DropStatement
	,ExecutionOrder
)
SELECT DISTINCT
	 CONCAT('DROP PROCEDURE [', s.name, '].[',p.name,']') AS SpDropStatement
	,1
FROM sys.schemas s
	INNER JOIN sys.procedures p
		ON p.schema_id = s.schema_id
WHERE s.name IN ('dbo')
	AND p.name IN
(
	 'sp_WhoIsActive'
	,'sp_BlitzFirst'
)

PRINT '-------------------------------------------------------------------'
PRINT 'Getting triggers'
PRINT '-------------------------------------------------------------------'

-- List of triggers to attempt to locate and drop before dropping the parent table
INSERT INTO @tblDropStatements
(
	 DropStatement
	,ExecutionOrder
)
SELECT DISTINCT
	 CONCAT('DROP TRIGGER [', s.name, '].[',r.name,']') AS TrDropStatement
	,2
FROM sys.schemas s
	INNER JOIN sys.tables t
		ON t.schema_id = s.schema_id
	INNER JOIN sys.triggers r
		ON r.parent_id = t.object_id
WHERE s.name = 'dbo' 
	AND r.name IN
(
		 'trTable1_Ins'
		,'trTable1_Upd'
		,'trTable1_Del'
)
	
PRINT '-------------------------------------------------------------------'
PRINT 'Getting constraints'
PRINT '-------------------------------------------------------------------'

-- List of constraints to attempt to locate and drop before dropping the parent table
INSERT INTO @tblDropStatements
(
	 DropStatement
	,ExecutionOrder
)
SELECT DISTINCT
	 CONCAT('ALTER TABLE [', s.name, '].[',t.name,'] DROP CONSTRAINT [',df.name,']') AS DfDropStatement
	,3
FROM sys.default_constraints df
	INNER JOIN sys.tables t
		ON t.object_id = df.parent_object_id
	INNER JOIN sys.schemas s
		ON s.schema_id = t.schema_id
WHERE s.name = 'dbo'
AND df.name IN
(
	  'DF_Table1_CreateOnUtc'
	 ,'DF_Table1_Col2'
	 ,'DF_Table1_Col3'
)
UNION
SELECT DISTINCT
	 CONCAT('ALTER TABLE [', s.name, '].[',t.name,'] DROP CONSTRAINT [',fk.name,']') AS FkDropStatement
	,4
FROM sys.foreign_keys fk
	INNER JOIN sys.tables t
		ON t.object_id = fk.parent_object_id
	INNER JOIN sys.schemas s
		ON s.schema_id = t.schema_id
WHERE s.name = 'dbo'
AND fk.name IN
(
	  'FK_Table1_Col1'
	 ,'FK_Table1_Col2'
	 ,'FK_Table1_Col2'
)
UNION
SELECT DISTINCT
	 CONCAT('ALTER TABLE [', s.name, '].[',t.name,'] DROP CONSTRAINT [',pk.name,']') AS PkDropStatement
	,5
FROM sys.key_constraints pk
	INNER JOIN sys.tables t
		ON t.object_id = pk.parent_object_id
	INNER JOIN sys.schemas s
		ON s.schema_id = t.schema_id
WHERE s.name IN ('dbo')
AND pk.name IN
(
	 'PK__Table1__6AD5A5F2'
	,'PK__Table2__68ED5D80'
	,'PK_Table3_Col1'
)

PRINT '-------------------------------------------------------------------'
PRINT 'Getting tables'
PRINT '-------------------------------------------------------------------'

-- List of tables to attempt to locate and drop, simple drop statements, they may or may not exist
INSERT INTO @tblDropStatements
(
	 DropStatement
	,ExecutionOrder
)
SELECT DISTINCT 
	 CONCAT(N'DROP TABLE [', s.name ,'].[', t.name, ']') AS TableDropStatement
	,6
FROM sys.tables t
	INNER JOIN sys.schemas s
		ON s.schema_id = t.schema_id
WHERE s.name = 'dbo'
	AND t.name IN
(
	 'Table1'
)
UNION
SELECT DISTINCT 
	 CONCAT(N'DROP TABLE [', s.name ,'].[', t.name, ']') AS TableDropStatement
	,7
FROM sys.tables t
	INNER JOIN sys.schemas s
		ON s.schema_id = t.schema_id
WHERE s.name = 'dbo2'
AND t.name IN
(
	 'Table2'
)

PRINT '-------------------------------------------------------------------'
PRINT 'Getting schema'
PRINT '-------------------------------------------------------------------'

-- List of tables to attempt to locate and drop, simple drop statements, they may or may not exist
INSERT INTO @tblDropStatements
(
	 DropStatement
	,ExecutionOrder
)
SELECT DISTINCT 
	 CONCAT(N'DROP SCHEMA [', s.name ,']') AS SchemaDropStatement
	,9
FROM sys.schemas s
WHERE s.name IN
(
	 'dbo2'
	,'xa'
	,'tbl'
)

IF(NOT EXISTS(SELECT TOP 1 1 FROM @tblDropStatements))
BEGIN
	PRINT 'There is nothing to drop. Script is exiting.'

	RETURN
END

SET TRANSACTION ISOLATION LEVEL Serializable

BEGIN TRY
	BEGIN TRANSACTION
	
	PRINT '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~'
	PRINT 'Execution drop loop'
	PRINT '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~'

	DECLARE @tbl TABLE
	(
		 EntryId INT IDENTITY(0, 1) NOT NULL PRIMARY KEY
		,DropStatement VARCHAR(2000) NOT NULL
	)

	-- Seeding the table with a sort order
	INSERT INTO @tbl
	(
		DropStatement
	)
	SELECT
		DropStatement
	FROM @tblDropStatements
	ORDER BY ExecutionOrder
	
	SELECT * FROM @tbl

	DECLARE @i INT = 0;
	DECLARE @count INT = (SELECT COUNT(EntryId) FROM @tbl);
	
	WHILE @i < @count
	BEGIN
		DECLARE @dropStatement VARCHAR(2000);
	
		SELECT @dropStatement = DropStatement FROM @tbl WHERE EntryId = @i;

		PRINT @dropStatement
		
		EXEC(@dropStatement);
		
		SET @i += 1;
	END

	COMMIT TRANSACTION
END TRY  
BEGIN CATCH
	ROLLBACK TRANSACTION

	;THROW;
END CATCH;