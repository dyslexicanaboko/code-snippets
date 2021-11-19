MERGE dbo.ExclusiveSlotSharedDate AS target
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
	ExclusiveSlotSharedDateId
)  
ON target.ExclusiveSlotSharedDateId = source.ExclusiveSlotSharedDateId
WHEN NOT MATCHED THEN  
	INSERT (ExclusiveSlotSharedDateId)  
	VALUES (source.ExclusiveSlotSharedDateId);
