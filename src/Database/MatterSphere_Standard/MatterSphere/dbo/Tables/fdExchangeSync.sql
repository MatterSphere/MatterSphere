CREATE TABLE [dbo].[fdExchangeSync] (
    [usrID]       INT            NOT NULL,
    [usrTimeZone] NVARCHAR (100) NULL,
    CONSTRAINT [PK_fdExchangeSync] PRIMARY KEY CLUSTERED ([usrID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[fdExchangeSync] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fdExchangeSync] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[fdExchangeSync] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[fdExchangeSync] TO [OMSApplicationRole]
    AS [dbo];

