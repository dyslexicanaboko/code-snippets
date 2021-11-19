CREATE PROCEDURE [dbo].[ResetTickets]
(
	@concertId INT
)	
AS
	UPDATE dbo.Ticket SET 
	 	 PurchaserId = NULL
		,PurchasedOn = NULL
	WHERE ConcertId = @concertId
RETURN 0
