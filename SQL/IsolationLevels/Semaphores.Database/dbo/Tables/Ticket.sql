CREATE TABLE [dbo].[Ticket]
(
[TicketId] [int] NOT NULL IDENTITY(1, 1),
[ConcertId] [int] NOT NULL,
[PurchaserId] [int] NULL,
[PurchasedOn] [datetime2] NULL CONSTRAINT [DF_dbo.Ticket_PurchasedOn] DEFAULT (getdate())
)
GO
ALTER TABLE [dbo].[Ticket] ADD CONSTRAINT [PK_dbo.Ticket_TicketId] PRIMARY KEY CLUSTERED  ([TicketId])
GO
ALTER TABLE [dbo].[Ticket] ADD CONSTRAINT [FK_dbo.Ticket_dbo.Concert_ConcertId] FOREIGN KEY ([ConcertId]) REFERENCES [dbo].[Concert] ([ConcertId])
GO
ALTER TABLE [dbo].[Ticket] ADD CONSTRAINT [FK_dbo.Ticket_dbo.Purchaser_PurchaserId] FOREIGN KEY ([PurchaserId]) REFERENCES [dbo].[Purchaser] ([PurchaserId])
GO
