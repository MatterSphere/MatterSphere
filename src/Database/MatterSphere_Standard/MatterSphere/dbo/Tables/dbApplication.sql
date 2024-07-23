CREATE TABLE [dbo].[dbApplication] (
    [appID]                 SMALLINT            NOT NULL,
    [appGUID]               UNIQUEIDENTIFIER    CONSTRAINT [DF_dbApplication_appGUID] DEFAULT (newid()) NULL,
    [appCode]               NVARCHAR (15)       NOT NULL,
    [appVersion]            VARCHAR (10)        NULL,
    [appName]               NVARCHAR (125)      NOT NULL,
    [appPath]               NVARCHAR (255)      NULL,
    [appProgID]             NVARCHAR (50)       NULL,
    [appPrintCmd]           NVARCHAR (12)       NULL,
    [appOpenCmd]            NVARCHAR (12)       NULL,
    [appAutomated]          BIT                 CONSTRAINT [DF_dbApplication_appIsAutomated] DEFAULT ((0)) NOT NULL,
    [appType]               NVARCHAR (125)      NULL,
    [appBlankPrecedent]     BIGINT              NULL,
    [appBlankPrecedentType] [dbo].[uCodeLookup] NULL,
    [appXml]                [dbo].[uXML]        CONSTRAINT [DF_dbApplication_appXml] DEFAULT (N'<config/>') NOT NULL,
    [rowguid]               UNIQUEIDENTIFIER    CONSTRAINT [DF_dbApplication_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbApplication] PRIMARY KEY CLUSTERED ([appID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbApplication_dbPrecedents] FOREIGN KEY ([appBlankPrecedent]) REFERENCES [dbo].[dbPrecedents] ([PrecID]) NOT FOR REPLICATION,
    CONSTRAINT [IX_dbApplication_appGuID] UNIQUE NONCLUSTERED ([appGUID] ASC) WITH (FILLFACTOR = 90) ON [IndexGroup],
    CONSTRAINT [IX_dbApplication_Code] UNIQUE NONCLUSTERED ([appCode] ASC, [appVersion] ASC) WITH (FILLFACTOR = 90) ON [IndexGroup]
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbApplication_rowguid]
    ON [dbo].[dbApplication]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO


CREATE TRIGGER [dbo].[tgrApplicationUpdated]
   ON  [dbo].[dbApplication] 
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	SET NOCOUNT ON;

	exec sprTableMonitorUpdate 'dbApplication'    

END


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbApplication] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbApplication] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbApplication] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbApplication] TO [OMSApplicationRole]
    AS [dbo];

