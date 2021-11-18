MERGE dbo.Concert AS target
USING (	VALUES
	('Slayer'),
	('Nine Inch Nails')
)
AS source 
(
	[Name]
)  
ON target.[Name] = source.[Name]
WHEN NOT MATCHED THEN  
	INSERT ([Name])  
	VALUES (source.[Name]);
