-- If the temporary table exists drop it
IF (EXISTS (SELECT * 
				 FROM INFORMATION_SCHEMA.TABLES 
				 WHERE TABLE_SCHEMA = '<TABLE_SCHEMA>' 
				 AND  TABLE_NAME = 'tmpBak_<TABLE_NAME>'))
BEGIN
	PRINT 'Table "<TABLE_SCHEMA>.tmpBak_<TABLE_NAME>" was found and is being dropped'
	
	DROP TABLE <TABLE_SCHEMA>.tmpBak_<TABLE_NAME>
END

BEGIN TRY
	BEGIN TRANSACTION

	-- Create a backup of the schema for seamless transition
	SELECT TOP 0 *
	INTO <TABLE_SCHEMA>.tmpBak_<TABLE_NAME>
	FROM <TABLE_SCHEMA>.<TABLE_NAME>

	/* ==========================================================
	Critical Execution Begin
	========================================================== */
	
	/*
	==========================================================
	Copy/Paste critical execution here
	
	Remember to operate on the temporary table
	==========================================================
	*/
	
	/* ==========================================================
	Critical Execution End
	========================================================== */
	
	-- Copy the data from the original table into the temporary table
	SET IDENTITY_INSERT <TABLE_SCHEMA>.tmpBak_<TABLE_NAME> ON

	INSERT INTO <TABLE_SCHEMA>.tmpBak_<TABLE_NAME>
	(
		<COLUMNS_NEW>
	)
	SELECT
		<COLUMNS_OLD>
	FROM <TABLE_SCHEMA>.<TABLE_NAME>

	SET IDENTITY_INSERT <TABLE_SCHEMA>.tmpBak_<TABLE_NAME> OFF
	
	-- Drop the original table
	DROP TABLE <TABLE_SCHEMA>.<TABLE_NAME>
	
	-- Rename the temporary table to the original table's name
	EXEC sp_rename '<TABLE_SCHEMA>.tmpBak_<TABLE_NAME>', '<TABLE_NAME>'; 
	
	COMMIT TRANSACTION
	
	PRINT 'Success'
END TRY
BEGIN CATCH
	PRINT 'Execution Failed - rolling back and stopping execution'
	
	ROLLBACK TRANSACTION
	
	SELECT  
		 ERROR_NUMBER() AS ErrorNumber  
		,ERROR_SEVERITY() AS ErrorSeverity  
		,ERROR_STATE() AS ErrorState  
		,ERROR_PROCEDURE() AS ErrorProcedure  
		,ERROR_LINE() AS ErrorLine  
		,ERROR_MESSAGE() AS ErrorMessage;
	
	RETURN
END CATCH	