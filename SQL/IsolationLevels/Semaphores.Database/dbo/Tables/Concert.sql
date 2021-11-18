CREATE TABLE [dbo].[Concert]
(
[ConcertId] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (50) NOT NULL
)
GO
ALTER TABLE [dbo].[Concert] ADD CONSTRAINT [PK_Concert] PRIMARY KEY CLUSTERED  ([ConcertId])
GO
