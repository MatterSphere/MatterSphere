CREATE TABLE [dbo].[dbScriptVersionData] (
    [VersionID] BIGINT           IDENTITY (1, 1) NOT NULL,
    [Code]      NVARCHAR (15)    NOT NULL,
    [Version]   BIGINT           NULL,
    [dbScript]  NVARCHAR (MAX)   NULL,
    [Created]   DATETIME         NULL,
    [CreatedBy] INT              NULL,
    [Comments]  NVARCHAR (MAX)   NULL,
    [rowguid]   UNIQUEIDENTIFIER CONSTRAINT [DF_dbScriptVersionData_rowguid] DEFAULT (newid()) ROWGUIDCOL NULL,
    CONSTRAINT [PK_dbScriptVersionData] PRIMARY KEY CLUSTERED ([VersionID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbScriptVersionData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbScriptVersionData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbScriptVersionData] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbScriptVersionData] TO [OMSApplicationRole]
    AS [dbo];

