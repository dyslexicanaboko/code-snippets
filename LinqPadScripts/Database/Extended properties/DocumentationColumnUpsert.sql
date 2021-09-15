CREATE PROCEDURE [dbo].[DocumentationColumnUpsert]
	 @schema sysname
	,@table sysname
	,@column sysname
	,@comments sysname
AS
	-- Constants
	DECLARE 
		 @name sysname = N'MS_Description'
		,@level0type VARCHAR(128) = N'SCHEMA'
		,@level1type VARCHAR(128) = N'TABLE'
		,@level2type VARCHAR(128) = N'COLUMN';

	IF EXISTS(
		SELECT TOP(1) 1 
		FROM sys.fn_listextendedproperty(
			 @name
			,@level0type
			,@schema
			,@level1type
			,@table
			,@level2type
			,@column))
	BEGIN
		EXEC sys.sp_updateextendedproperty 
			 @name = @name
			,@value = @comments
			,@level0type = @level0type
			,@level0name = @schema
			,@level1type = @level1type
			,@level1name = @table
			,@level2type = @level2type
			,@level2name = @column;
	END
	ELSE
	BEGIN
		EXEC sys.sp_addextendedproperty 
			 @name = @name
			,@value = @comments
			,@level0type = @level0type
			,@level0name = @schema
			,@level1type = @level1type
			,@level1name = @table
			,@level2type = @level2type
			,@level2name = @column;
	END
RETURN 0
