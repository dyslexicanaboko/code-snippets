MERGE dbo.ExclusiveSlot AS target
USING (	VALUES
	(1),
	(2),
	(3),
	(4),
	(5),
	(6),
	(7),
	(8),
	(9),
	(10)
)
AS source 
(
	ExclusiveSlotId
)  
ON target.ExclusiveSlotId = source.ExclusiveSlotId
WHEN NOT MATCHED THEN  
	INSERT (ExclusiveSlotId)  
	VALUES (source.ExclusiveSlotId);
