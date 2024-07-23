CREATE TABLE [dbo].[dbArchiveParties](
		[archPartyID] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
		[archID] [bigint] NOT NULL,
		[archPartyType] [nvarchar](5) NULL,
		[archMCID] [bigint] NULL,
		[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_dbArchiveParties_rowguid]  DEFAULT (newid()),
	 CONSTRAINT [PK_dbArchiveParties] PRIMARY KEY CLUSTERED 
	(
		[archPartyID] ASC
	)
	)

GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbArchiveParties] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbArchiveParties] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbArchiveParties] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbArchiveParties] TO [OMSApplicationRole]
    AS [dbo];
