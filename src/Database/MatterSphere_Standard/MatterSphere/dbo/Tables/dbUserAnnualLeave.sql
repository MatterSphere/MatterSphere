CREATE TABLE [dbo].[dbUserAnnualLeave] (
    [usrID]   INT              NOT NULL,
    [alDate]  SMALLDATETIME    NOT NULL,
    [rowguid] UNIQUEIDENTIFIER CONSTRAINT [DF_dbUserAnnualLeave_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbUserAnnualLeave] PRIMARY KEY CLUSTERED ([usrID] ASC, [alDate] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbUserAnnualLeave]
    ON [dbo].[dbUserAnnualLeave]([rowguid] ASC)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbUserAnnualLeave] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbUserAnnualLeave] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbUserAnnualLeave] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbUserAnnualLeave] TO [OMSApplicationRole]
    AS [dbo];

