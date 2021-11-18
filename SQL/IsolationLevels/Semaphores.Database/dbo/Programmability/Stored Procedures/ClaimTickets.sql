CREATE PROCEDURE [dbo].[ClaimTickets]
	 @concertId INT
	,@purchaserId INT
	,@amount INT
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
		DECLARE @tblAvailableTickets TABLE
		(
			TicketId INT NOT NULL	
		);

		-- Get the tickets that are available as of right now
		-- but only get the amount required
		INSERT INTO @tblAvailableTickets
		SELECT TOP (@amount) t.TicketId 
		FROM dbo.Ticket t 
		WHERE t.ConcertId = @concertId 
			AND t.PurchaserId IS NULL
		ORDER BY t.TicketId

		IF NOT EXISTS(SELECT * FROM @tblAvailableTickets)
		BEGIN;
			THROW 51000, 'All tickets have been sold!.', 1;
		END

		-- Nab the tickets for this purchaser	
		UPDATE t SET
			 t.PurchaserId = @purchaserId
			,t.PurchasedOn = GETDATE()
		FROM dbo.Ticket t
			INNER JOIN @tblAvailableTickets a
				ON a.TicketId = t.TicketId

		SELECT
			t.TicketId,
			t.ConcertId,
			c.[Name] AS Concert,
			t.PurchaserId,
			t.PurchasedOn
		FROM dbo.Ticket t
			INNER JOIN dbo.Concert c
				ON c.ConcertId = t.ConcertId
		WHERE t.PurchaserId = @purchaserId

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;

		THROW;
	END CATCH
RETURN 0
