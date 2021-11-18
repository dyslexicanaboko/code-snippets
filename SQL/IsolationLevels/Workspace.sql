SELECT
    t.TicketId,
	t.ConcertId,
    c.[Name] AS Concert,
    t.PurchaserId,
    t.PurchasedOn
FROM dbo.Ticket t
	INNER JOIN dbo.Concert c
		ON c.ConcertId = t.ConcertId
ORDER BY t.PurchaserId
--ORDER BY t.PurchasedOn DESC

RETURN

-- Undo all the sales so I can test again
UPDATE dbo.Ticket SET 
	 PurchaserId = NULL
	,PurchasedOn = NULL
WHERE ConcertId = 1

--ALTER DATABASE Semaphores SET ALLOW_SNAPSHOT_ISOLATION ON
--ALTER DATABASE Semaphores SET ALLOW_SNAPSHOT_ISOLATION OFF

SELECT * FROM dbo.Concert
SELECT * FROM dbo.Ticket
SELECT * FROM dbo.Purchaser

SELECT
	p.PurchaserId,
    p.FirstName,
    p.LastName,
    t.TicketId,
    t.ConcertId,
    t.PurchaserId,
    t.PurchasedOn
FROM dbo.Purchaser p
	LEFT JOIN dbo.Ticket t
		ON p.PurchaserId = t.PurchaserId
ORDER BY t.TicketId


SELECT
	COUNT(p.PurchaserId) AS Sold
FROM dbo.Purchaser p
	INNER JOIN dbo.Ticket t
		ON p.PurchaserId = t.PurchaserId

