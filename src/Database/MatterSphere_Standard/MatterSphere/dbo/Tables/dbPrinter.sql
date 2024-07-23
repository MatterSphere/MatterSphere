CREATE TABLE [dbo].[dbPrinter] (
    [printID]               INT               IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [printName]             NVARCHAR (100)    NOT NULL,
    [printUNCName]          [dbo].[uFilePath] NULL,
    [printLocation]         NVARCHAR (50)     NULL,
    [printDescription]      NVARCHAR (100)    NULL,
    [printTrays]            TINYINT           CONSTRAINT [DF_dbPrinter_printTrays] DEFAULT ((0)) NOT NULL,
    [printCopyTray]         NVARCHAR (50)     NULL,
    [printLetterheadTray]   NVARCHAR (50)     NULL,
    [printBillPaperTray]    NVARCHAR (50)     NULL,
    [printEngrossmentTray]  NVARCHAR (50)     NULL,
    [printColouredTray]     NVARCHAR (50)     NULL,
    [printDefaultTray]      NVARCHAR (50)     NULL,
    [printContinuationTray] NVARCHAR (50)     NULL,
    [printOnlyatBranch]     INT               NULL,
    [printXML]              [dbo].[uXML]      CONSTRAINT [DF_dbPrinter_printXML] DEFAULT (N'<PRINTER>
	</PRINTER>') NULL,
    [printInstallCmd]       NVARCHAR (200)    NULL,
    [rowguid]               UNIQUEIDENTIFIER  CONSTRAINT [DF_dbPrinter_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbPrinter] PRIMARY KEY CLUSTERED ([printID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbPrinter_rowguid]
    ON [dbo].[dbPrinter]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbPrinter] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbPrinter] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbPrinter] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbPrinter] TO [OMSApplicationRole]
    AS [dbo];

