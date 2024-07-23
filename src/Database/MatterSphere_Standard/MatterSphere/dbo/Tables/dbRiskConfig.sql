CREATE TABLE [dbo].[dbRiskConfig] (
    [riskID]          INT                 IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [riskCode]        NVARCHAR (15)       NOT NULL,
    [riskDescription] NVARCHAR (255)      NOT NULL,
    [riskActive]      BIT                 CONSTRAINT [DF_dbRiskConfig_rskActive] DEFAULT ((1)) NOT NULL,
    [riskDept]        [dbo].[uCodeLookup] NULL,
    [rowguid]         UNIQUEIDENTIFIER    CONSTRAINT [DF_dbRiskConfig_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbRiskConfig] PRIMARY KEY CLUSTERED ([riskID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbRiskConfig_dbDepartment] FOREIGN KEY ([riskDept]) REFERENCES [dbo].[dbDepartment] ([deptCode]) NOT FOR REPLICATION,
    CONSTRAINT [IX_dbRiskConfig_riskCode] UNIQUE NONCLUSTERED ([riskCode] ASC) WITH (FILLFACTOR = 90) ON [IndexGroup]
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbRiskConfig_rowguid]
    ON [dbo].[dbRiskConfig]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbRiskConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbRiskConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbRiskConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbRiskConfig] TO [OMSApplicationRole]
    AS [dbo];

