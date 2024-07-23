CREATE TABLE [dbo].[dbObjectLockingAudit] (
    [id]         BIGINT           IDENTITY (1, 1) NOT NULL,
    [ObjectCode] NVARCHAR (15)    NULL,
    [ObjectType] NVARCHAR (15)    NULL,
    [Locked]     DATETIME         NULL,
    [LockedBy]   INT              NULL,
    [rowguid]    UNIQUEIDENTIFIER CONSTRAINT [DF_dbObjectLockingAudit_rowguid] DEFAULT (newid()) NULL,
    CONSTRAINT [PK_dbObjectLockingAudit] PRIMARY KEY CLUSTERED ([id] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbObjectLockingAudit] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbObjectLockingAudit] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbObjectLockingAudit] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbObjectLockingAudit] TO [OMSApplicationRole]
    AS [dbo];

