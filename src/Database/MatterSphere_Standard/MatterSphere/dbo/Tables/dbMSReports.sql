CREATE TABLE [dbo].[dbMSReports] (
    [ID]          UNIQUEIDENTIFIER CONSTRAINT [DF_dbMSReports_ID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [ReportName]  NVARCHAR (2000)  NULL,
    [Description] NVARCHAR (250)   NULL,
    CONSTRAINT [PK_dbMSReports] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbMSReports] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbMSReports] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbMSReports] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbMSReports] TO [OMSApplicationRole]
    AS [dbo];

