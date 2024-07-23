CREATE TABLE [dbo].[dbClientStatus] (
    [ID]             INT                 IDENTITY (1, 1) NOT NULL,
    [csCode]         [dbo].[uCodeLookup] NULL,
    [csAccCode]      NVARCHAR (50)       NULL,
    [csTimeEntry]    BIT                 NULL,
    [csFileCreation] BIT                 NULL,
    [rowguid]        UNIQUEIDENTIFIER    CONSTRAINT [DF_dbClientStatus_rowguid] DEFAULT (newid()) ROWGUIDCOL NULL,
    [Created]        [dbo].[uCreated]    NULL,
    [CreatedBy]      [dbo].[uCreatedBy]  NULL,
    [Updated]        [dbo].[uCreated]    NULL,
    [UpdatedBy]      [dbo].[uCreatedBy]  NULL,
    CONSTRAINT [PK_dbClientStatus] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [UV_dbClientStatus_csCode] UNIQUE NONCLUSTERED ([csCode] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbClientStatus] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbClientStatus] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbClientStatus] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbClientStatus] TO [OMSApplicationRole]
    AS [dbo];

