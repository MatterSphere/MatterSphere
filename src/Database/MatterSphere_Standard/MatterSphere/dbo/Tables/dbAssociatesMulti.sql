CREATE TABLE [dbo].[dbAssociatesMulti] (
    [multiID]               BIGINT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [fileType]              [dbo].[uCodeLookup] NOT NULL,
    [contID]                BIGINT              NOT NULL,
    [multiType]             [dbo].[uCodeLookup] NOT NULL,
    [multiDefRef]           NVARCHAR (50)       NULL,
    [multiDefHeading1]      NVARCHAR (50)       NULL,
    [multiDefHeadingField1] NVARCHAR (50)       NULL,
    [multiDefHeading2]      NVARCHAR (50)       NULL,
    [multiDefHeadingField2] NVARCHAR (50)       NULL,
    [multiDefHeading3]      NVARCHAR (50)       NULL,
    [multiDefHeadingField3] NVARCHAR (50)       NULL,
    [multiDefSalut]         NVARCHAR (35)       NULL,
    [multiDefAddressee]     NVARCHAR (50)       NULL,
    [multiDefAddress]       BIGINT              NULL,
    [multiDefDDI]           [dbo].[uTelephone]  NULL,
    [multiDefMobile]        [dbo].[uTelephone]  NULL,
    [multiDefFax]           [dbo].[uTelephone]  NULL,
    [multiDefEmail]         [dbo].[uEmail]      NULL,
    [rowguid]               UNIQUEIDENTIFIER    CONSTRAINT [DF_dbAssociatesMulti_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbMultiAssocs] PRIMARY KEY CLUSTERED ([multiID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbAssociatesMulti_dbContact] FOREIGN KEY ([contID]) REFERENCES [dbo].[dbContact] ([contID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbAssociatesMulti_dbFileType] FOREIGN KEY ([fileType]) REFERENCES [dbo].[dbFileType] ([typeCode]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbAssociatesMulti_rowguid]
    ON [dbo].[dbAssociatesMulti]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbAssociatesMulti] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbAssociatesMulti] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbAssociatesMulti] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbAssociatesMulti] TO [OMSApplicationRole]
    AS [dbo];

