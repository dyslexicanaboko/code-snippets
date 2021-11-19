CREATE PROCEDURE [dbo].[ResetExclusiveSlots]
AS
	UPDATE dbo.ExclusiveSlot SET
		 Slot0 = NULL
		,Slot1 = NULL
		,Slot2 = NULL
		,Slot3 = NULL
		,Slot4 = NULL
		,Slot5 = NULL
		,Slot6 = NULL
		,Slot7 = NULL
		,Slot8 = NULL
		,Slot9 = NULL
		,[Index] = NULL
		,[Value] = NULL
	WHERE 1 = 1 -- Make RedGate shut up
RETURN 0
