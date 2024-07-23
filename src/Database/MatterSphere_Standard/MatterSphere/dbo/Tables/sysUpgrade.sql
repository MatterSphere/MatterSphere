CREATE TABLE [dbo].[sysUpgrade] (
    [upID]         INT             NOT NULL,
    [upCreated]    DATETIME        NOT NULL,
    [upException]  NVARCHAR (MAX)  NULL,
    [upLog]        NVARCHAR (2000) NOT NULL,
    [upProcedures] NVARCHAR (MAX)  NULL,
    [upViews]      NVARCHAR (MAX)  NULL,
    [upFunctions]  NVARCHAR (MAX)  NULL,
    [upTables]     NVARCHAR (MAX)  NULL,
    [upVersion]    NVARCHAR (50)   NOT NULL,
    CONSTRAINT [PK_sysUpgrade] PRIMARY KEY CLUSTERED ([upID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[sysUpgrade] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[sysUpgrade] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[sysUpgrade] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[sysUpgrade] TO [OMSApplicationRole]
    AS [dbo];

