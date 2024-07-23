CREATE TABLE [dbo].[dbRiskQuestion] (
    [qID]         INT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [qriskID]     NVARCHAR (12)    NOT NULL,
    [qNumber]     TINYINT          CONSTRAINT [DF_DBRiskQuestion_qNumber] DEFAULT ((0)) NOT NULL,
    [qDesc]       NVARCHAR (255)   NULL,
    [qFormat]     INT              NULL,
    [qWeight]     INT              NULL,
    [qScaleStart] INT              NULL,
    [qScaleEnd]   INT              NULL,
    [riskHelp]    NVARCHAR (200)   NULL,
    [rowguid]     UNIQUEIDENTIFIER CONSTRAINT [DF_dbRiskQuestion_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_DBRiskQuestion] PRIMARY KEY CLUSTERED ([qID] ASC, [qriskID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbRiskQuestion_rowguid]
    ON [dbo].[dbRiskQuestion]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbRiskQuestion] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbRiskQuestion] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbRiskQuestion] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbRiskQuestion] TO [OMSApplicationRole]
    AS [dbo];

