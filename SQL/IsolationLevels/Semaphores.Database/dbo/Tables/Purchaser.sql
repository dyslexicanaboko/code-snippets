CREATE TABLE [dbo].[Purchaser]
(
[PurchaserId] [int] NOT NULL IDENTITY(1, 1),
[FirstName] [varchar] (100) NOT NULL,
[LastName] [varchar] (100) NOT NULL
)
GO
ALTER TABLE [dbo].[Purchaser] ADD CONSTRAINT [PK_dbo.Purchaser_PurchaserId] PRIMARY KEY CLUSTERED  ([PurchaserId])
GO
