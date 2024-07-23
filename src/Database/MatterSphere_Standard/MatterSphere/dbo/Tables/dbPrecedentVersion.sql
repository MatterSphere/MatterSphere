CREATE TABLE [dbo].[dbPrecedentVersion] (
    [verID]              UNIQUEIDENTIFIER CONSTRAINT [DF_dbPrecedentVersion_verID] DEFAULT (newid()) NOT NULL,
    [precID]             BIGINT           NOT NULL,
    [verNumber]          INT              CONSTRAINT [DF_dbPrecedentVersion_verNumber] DEFAULT ((0)) NOT NULL,
    [verParent]          UNIQUEIDENTIFIER NULL,
    [verDepth]           TINYINT          CONSTRAINT [DF_dbPrecedentVersion_verDepth] DEFAULT ((0)) NOT NULL,
    [verLabel]           NVARCHAR (50)    NULL,
    [verStatus]          NVARCHAR (15)    NULL,
    [verComments]        NVARCHAR (500)   NULL,
    [verToken]           NVARCHAR (255)   NULL,
    [verChecksum]        VARCHAR (50)     NULL,
    [verLastEditedBy]    INT              NULL,
    [verLastEdited]      DATETIME         NULL,
    [verAuthoredBy]      INT              NULL,
    [verAuthored]        DATETIME         NULL,
    [CreatedBy]          INT              NULL,
    [Created]            DATETIME         NULL,
    [UpdatedBy]          INT              NULL,
    [Updated]            DATETIME         NULL,
    [verDeleted]         BIT              CONSTRAINT [DF_dbPrecedentVersion_verDeleted] DEFAULT ((0)) NOT NULL,
    [verScriptVersionID] BIGINT           NULL,
    [rowguid]            UNIQUEIDENTIFIER CONSTRAINT [DF_dbPrecedentVersion_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbPrecedentVersion] PRIMARY KEY CLUSTERED ([verID] ASC),
    CONSTRAINT [FK_dbPrecedentVersion_dbPrecedentVersion] FOREIGN KEY ([verParent]) REFERENCES [dbo].[dbPrecedentVersion] ([verID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbPrecedentVersion_dbPrecedentVersion1] FOREIGN KEY ([verID]) REFERENCES [dbo].[dbPrecedentVersion] ([verID]) NOT FOR REPLICATION
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbPrecedentVersion] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbPrecedentVersion] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbPrecedentVersion] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbPrecedentVersion] TO [OMSApplicationRole]
    AS [dbo];

