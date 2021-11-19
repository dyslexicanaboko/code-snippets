CREATE PROCEDURE [dbo].[FillExclusiveSlot]
	 @exclusiveSlotId INT
	,@slotIndex INT
	,@value INT
AS
	/*
		{ READ UNCOMMITTED
		| READ COMMITTED
		| REPEATABLE READ
		| SNAPSHOT
		| SERIALIZABLE }

		Snapshot isolation requires it to be enabled:
			ALTER DATABASE Semaphores SET ALLOW_SNAPSHOT_ISOLATION ON 
			ALTER DATABASE Semaphores SET ALLOW_SNAPSHOT_ISOLATION OFF 
	*/

	SET TRANSACTION ISOLATION LEVEL READ COMMITTED

	BEGIN TRANSACTION;

	BEGIN TRY
		-- Fill the appropriate slot indicated by the inputs
		DECLARE @sql NVARCHAR(2000) = CONCAT('
		UPDATE dbo.ExclusiveSlot SET
			  Slot', CAST(@slotIndex AS VARCHAR(10)),' = @value
			 ,[Index] = @slotIndex
			 ,[Value] = @value
		WHERE ExclusiveSlotId = @exclusiveSlotId');

		EXEC sp_executesql @sql
			,N'@exclusiveSlotId INT, @slotIndex INT, @value INT'
			,@exclusiveSlotId = @exclusiveSlotId
			,@slotIndex = @slotIndex
			,@value = @value;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;

		THROW;
	END CATCH
RETURN 0
