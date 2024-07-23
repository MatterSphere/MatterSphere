CREATE TABLE [dbo].[dbPrecedentTeamsAccess](
	[teamID] [int] NOT NULL,
	[precID] [bigint] NOT NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL NOT NULL,
 CONSTRAINT [PK_dbPrecedentTeamsAccess] PRIMARY KEY CLUSTERED ([precID], [teamID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY])
GO

ALTER TABLE [dbo].[dbPrecedentTeamsAccess] ADD  CONSTRAINT [DF_dbPrecedentTeamsAccess_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO

