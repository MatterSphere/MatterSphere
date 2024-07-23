CREATE TABLE [dbo].[dbRisks] (
    [datID]             INT                NOT NULL,
    [datriskID]         INT                NOT NULL,
    [fileID]            BIGINT             NULL,
    [datValue]          SMALLMONEY         NULL,
    [datcurID]          NVARCHAR (12)      NULL,
    [datCalculation]    INT                NULL,
    [datTotalWeight]    INT                NULL,
    [datLastCalculated] SMALLDATETIME      NULL,
    [datReferredTo]     BIGINT             NULL,
    [datAcceptedBy]     INT                NULL,
    [datNotes]          NVARCHAR (200)     NULL,
    [Created]           [dbo].[uCreated]   NULL,
    [Createdby]         [dbo].[uCreatedBy] NULL,
    [rowguid]           UNIQUEIDENTIFIER   CONSTRAINT [DF_dbRisks_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbRisks] PRIMARY KEY CLUSTERED ([datID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbRisks_rowguid]
    ON [dbo].[dbRisks]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbRisks] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbRisks] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbRisks] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbRisks] TO [OMSApplicationRole]
    AS [dbo];

