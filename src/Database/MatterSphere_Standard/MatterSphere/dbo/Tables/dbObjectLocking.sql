CREATE TABLE [dbo].[dbObjectLocking] (
    [ObjectCode] NVARCHAR (15)  NOT NULL,
    [ObjectType] NVARCHAR (15)  NOT NULL,
    [ObjectOpen] BIT            NULL,
    [Locked]     DATETIME       NULL,
    [LockedBy]   INT            NULL,
    [rowguid]    UNIQUEIDENTIFIER CONSTRAINT [DF_dbObjectLocking_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_dbObjectLocking] PRIMARY KEY CLUSTERED ([ObjectCode] ASC, [ObjectType] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbObjectLocking] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbObjectLocking] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbObjectLocking] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbObjectLocking] TO [OMSApplicationRole]
    AS [dbo];

