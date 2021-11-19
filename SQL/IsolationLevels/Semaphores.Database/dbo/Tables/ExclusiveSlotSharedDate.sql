CREATE TABLE [dbo].[ExclusiveSlotSharedDate]
(
	 [ExclusiveSlotSharedDateId] INT NOT NULL
	,[Slot0] INT NULL
	,[Slot1] INT NULL
	,[Slot2] INT NULL
	,[Slot3] INT NULL
	,[Slot4] INT NULL
	,[Slot5] INT NULL
	,[Slot6] INT NULL
	,[Slot7] INT NULL
	,[Slot8] INT NULL
	,[Slot9] INT NULL
	,[UpdatedOnUtc] DATETIME2(7) NULL
	,CONSTRAINT [PK_dbo.ExclusiveSlotSharedDate_ExclusiveSlotSharedDateId] PRIMARY KEY CLUSTERED ([ExclusiveSlotSharedDateId])
)
