CREATE TABLE [dbo].[dbUfn] (
    [UFNID]          INT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CLID]           BIGINT           NOT NULL,
    [ufnCode]        NVARCHAR (20)    NULL,
    [ufnCLCode]      NVARCHAR (12)    NULL,
    [ufnOffCode]     NVARCHAR (12)    NULL,
    [ufnOutCode]     NVARCHAR (12)    NULL,
    [ufnConcluded]   DATETIME         NULL,
    [ufnSuspects]    INT              NULL,
    [ufnAttendances] INT              NULL,
    [ufnIdentifier]  NVARCHAR (50)    NULL,
    [ufnDuty]        BIT              NULL,
    [ufnYouth]       BIT              NULL,
    [ufnProCosts]    MONEY            NULL,
    [ufnDisb]        MONEY            NULL,
    [ufnTravel]      MONEY            NULL,
    [ufnWaiting]     MONEY            NULL,
    [ufnHead]        BIT              NULL,
    [ufnHeadCode]    NVARCHAR (20)    NULL,
    [ufnClass]       NVARCHAR (12)    NULL,
    [rowguid]        UNIQUEIDENTIFIER CONSTRAINT [DF_dbUfn_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbUfn] PRIMARY KEY CLUSTERED ([UFNID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbUfn_rowguid]
    ON [dbo].[dbUfn]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE NONCLUSTERED INDEX [IX_dbUfn_UfnHeadCode]
    ON [dbo].[dbUfn]([ufnHeadCode] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbUfn] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbUfn] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbUfn] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbUfn] TO [OMSApplicationRole]
    AS [dbo];

