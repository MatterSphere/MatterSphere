CREATE TABLE [dbo].[dbMilestoneConfig] (
    [msCode]           [dbo].[uCodeLookup] NOT NULL,
    [msDescription]    NVARCHAR (100)      NOT NULL,
    [msDepartment]     [dbo].[uCodeLookup] NULL,
    [msAllDepartments] BIT                 CONSTRAINT [DF_dbMilestone_msAllDepartments] DEFAULT ((1)) NOT NULL,
    [Created]          [dbo].[uCreated]    NULL,
    [CreatedBy]        [dbo].[uCreatedBy]  NULL,
    [Updated]          [dbo].[uCreated]    NULL,
    [UpdatedBy]        [dbo].[uCreatedBy]  NULL,
    [rowguid]          UNIQUEIDENTIFIER    CONSTRAINT [DF_dbMilestoneConfig_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbMilestoneConfig] PRIMARY KEY CLUSTERED ([msCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbMilestoneConfig_rowguid]
    ON [dbo].[dbMilestoneConfig]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbMilestoneConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbMilestoneConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbMilestoneConfig] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbMilestoneConfig] TO [OMSApplicationRole]
    AS [dbo];

