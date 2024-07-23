CREATE TABLE [dbo].[dbFileStatus] (
    [ID]                INT                 IDENTITY (1, 1) NOT NULL,
    [fsCode]            [dbo].[uCodeLookup] NULL,
    [fsAccCode]         NVARCHAR (50)       NULL,
    [fsTimeEntry]       BIT                 NULL,
    [fsDocModification] BIT                 NULL,
    [fsAppCreation]     BIT                 NULL,
    [fsAssocCreation]   BIT                 NULL,
    [fsTaskflowEdit]    BIT                 NULL,
    [fsAlertLevel]      SMALLINT            NULL,
    [rowguid]           UNIQUEIDENTIFIER    CONSTRAINT [DF_dbFileStatus_rowguid] DEFAULT (newid()) ROWGUIDCOL NULL,
    [Created]           [dbo].[uCreated]    NULL,
    [CreatedBy]         [dbo].[uCreatedBy]  NULL,
    [Updated]           [dbo].[uCreated]    NULL,
    [UpdatedBy]         [dbo].[uCreatedBy]  NULL,
    CONSTRAINT [PK_dbFileStatus] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [UV_dbFileStatus_csCode] UNIQUE NONCLUSTERED ([fsCode] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbFileStatus] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbFileStatus] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbFileStatus] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbFileStatus] TO [OMSApplicationRole]
    AS [dbo];

